using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using US_Txes_WebAPI_Core.DbModels;
using US_Txes_WebAPI_Core.Enumerables;
using US_Txes_WebAPI_Core.Extensions;
using US_Txes_WebAPI_Core.Models;

namespace US_Txes_WebAPI_Core.DbRepositories
{
    public class TaxesRepository : ITaxesRepository
    {
        private readonly CustomDbContext _db;
        public TaxesRepository(CustomDbContext db)
        {
            _db = db;
        }
        public async Task<IEnumerable<Tax>> CalculateAllTaxes()
        {
            var statesDb = await _db.States
                .Include(s => s.ZipCodes).ThenInclude(zc => zc.Fee)
                .Include(s => s.VehicleFees)
                .ToListAsync();

            var result = new List<Tax>();

            if (statesDb.IsAny())
            {
                foreach (var state in statesDb)
                {
                    foreach (var zipCode in state.ZipCodes)
                    {
                        if (zipCode.Fee != null)
                        {
                            foreach (var vehicleFee in state.VehicleFees)
                            {
                                var tax = new Tax();
                                {
                                    tax.StateAbbreviation = state.Abbreviation;
                                    tax.ZipCode = zipCode.Value;
                                    tax.VehicleType = vehicleFee.Type;
                                    tax.Vehicle = vehicleFee.Type.ToString();
                                    tax.Remarks = vehicleFee.IsAvailable ? null : vehicleFee.Remarks;
                                    tax.Payment = vehicleFee.IsAvailable ? Math.Round(vehicleFee.Koefficient.Value * zipCode.Fee.Value, 2) : default(double?);
                                }

                                result.Add(tax);
                            }
                        }
                    }
                }
            }

            return result;
        }

        public async Task<Tax> CalculateTax(string stateAbbreviation, int zipCode, VehicleTypes vehicleType)
        {
            var stateDb = await _db.States
                .Include(s => s.ZipCodes).ThenInclude(zc => zc.Fee)
                .Include(s => s.VehicleFees)
                .FirstOrDefaultAsync(s => s.Abbreviation.ToLower() == stateAbbreviation.ToLower());

            var result = new Tax();
            result.StateAbbreviation = stateAbbreviation;
            result.ZipCode = zipCode;
            result.VehicleType = vehicleType;
            result.Vehicle = vehicleType.ToString();

            if (stateDb == null)
            {
                result.Remarks = "Specified state was not found";
                result.Payment = null;

                return result;
            }
            else
            {
                double vehicleFeeValue = 0;
                double feeValue = 0;

                if (!stateDb.VehicleFees.IsAny())
                {
                    result.Remarks = "VehicleFees are not configured for specified state";
                    result.Payment = 0;

                    return result;
                }
                else
                {
                    var knownVehicleFee = stateDb.VehicleFees.FirstOrDefault(vf => vf.Type == vehicleType);
                    if (knownVehicleFee == null)
                    {
                        result.Remarks = "Specified VehicleFee type was not configured";
                        result.Payment = null;

                        return result;
                    }
                    else
                    {
                        if (!knownVehicleFee.IsAvailable)
                        {
                            result.Remarks = knownVehicleFee.Remarks;
                            result.Payment = null;

                            return result;
                        }
                        else
                        {
                            vehicleFeeValue = knownVehicleFee.Koefficient.Value;
                        }
                    }
                }

                if (!stateDb.ZipCodes.IsAny())
                {
                    result.Remarks = "ZipCodes are not configured for specified state";
                    result.Payment = 0;

                    return result;
                }
                else
                {
                    var knownZipCode = stateDb.ZipCodes.FirstOrDefault(vf => vf.Value == zipCode);
                    if (knownZipCode == null)
                    {
                        result.Remarks = "Specified ZipCode was not configured";
                        result.Payment = null;

                        return result;
                    }
                    else
                    {
                        if (knownZipCode.Fee == null)
                        {
                            result.Remarks = "Fee was not configured for specified ZipCode";
                            result.Payment = null;

                            return result;
                        }
                        else
                        {
                            feeValue = knownZipCode.Fee.Value;
                        }
                    }
                }

                result.Payment = Math.Round(feeValue * vehicleFeeValue, 2);
            }

            return result;
        }
    }
}
