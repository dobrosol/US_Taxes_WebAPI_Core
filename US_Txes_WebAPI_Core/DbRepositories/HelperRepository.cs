using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using US_Txes_WebAPI_Core.DbModels;
using US_Txes_WebAPI_Core.Enumerables;
using US_Txes_WebAPI_Core.Extensions;

namespace US_Txes_WebAPI_Core.DbRepositories
{
    public class HelperRepository : IHelperRepository
    {
        private readonly CustomDbContext _db;
        public HelperRepository(CustomDbContext db)
        {
            _db = db;
        }
        public async Task InitializeData()
        {
            await RemoveAllData();

            await InsertAllData();
        }

        private async Task RemoveAllData()
        {
            var existingVehicleFees = await _db.VehicleFees.ToListAsync();

            if (existingVehicleFees.IsAny())
            {
                _db.VehicleFees.RemoveRange(existingVehicleFees);
            }
            await _db.SaveChangesAsync();

            var existingFees = await _db.Fees.ToListAsync();
            if (existingFees.IsAny())
            {
                _db.Fees.RemoveRange(existingFees);
            }
            await _db.SaveChangesAsync();

            var existingZipCodes = await _db.ZipCodes.ToListAsync();
            if (existingZipCodes.IsAny())
            {
                _db.ZipCodes.RemoveRange(existingZipCodes);
            }
            await _db.SaveChangesAsync();

            var existingStates = await _db.States.ToListAsync();
            if (existingStates.IsAny())
            {
                _db.States.RemoveRange(existingStates);
            }
            await _db.SaveChangesAsync();
        }

        private async Task InsertAllData()
        {
            var statesDb = new List<StateDb>()
            {
                new StateDb()
                {
                    Abbreviation = "CA",
                    Name = "California",
                    VehicleFees = new List<VehicleFeeDb>()
                    {
                        new VehicleFeeDb()
                        {
                            IsAvailable = true,
                            Koefficient = 1.05,
                            Remarks = null,
                            Type = VehicleTypes.Passenger
                        },
                        new VehicleFeeDb()
                        {
                            IsAvailable = true,
                            Koefficient = 1.2,
                            Remarks = null,
                            Type = VehicleTypes.Truck
                        },
                        new VehicleFeeDb()
                        {
                            IsAvailable = true,
                            Koefficient = 1.25,
                            Remarks = null,
                            Type = VehicleTypes.Trailer
                        }
                    },
                    ZipCodes = new List<ZipCodeDb>()
                    {
                        new ZipCodeDb()
                        {
                            Value = 90011,
                            Fee = new FeeDb(){ Value = 100 }
                        },
                        new ZipCodeDb()
                        {
                            Value = 90044,
                            Fee = new FeeDb(){ Value = 120 }
                        },
                        new ZipCodeDb()
                        {
                            Value = 90201,
                            Fee = new FeeDb(){ Value = 130 }
                        },
                        new ZipCodeDb()
                        {
                            Value = 90650,
                            Fee = new FeeDb(){ Value = 110 }
                        },
                        new ZipCodeDb()
                        {
                            Value = 91131,
                            Fee = new FeeDb(){ Value = 90 }
                        }
                    }
                },
                new StateDb()
                {
                    Abbreviation = "FL",
                    Name = "Florida",
                    VehicleFees = new List<VehicleFeeDb>()
                    {
                        new VehicleFeeDb()
                        {
                            IsAvailable = true,
                            Koefficient = 1.1,
                            Remarks = null,
                            Type = VehicleTypes.Passenger
                        },
                        new VehicleFeeDb()
                        {
                            IsAvailable = true,
                            Koefficient = 1.5,
                            Remarks = null,
                            Type = VehicleTypes.Truck
                        },
                        new VehicleFeeDb()
                        {
                            IsAvailable = false,
                            Koefficient = null,
                            Remarks = "Registration not supported for the Trailer in FL",
                            Type = VehicleTypes.Trailer
                        }
                    },
                    ZipCodes = new List<ZipCodeDb>()
                    {
                        new ZipCodeDb()
                        {
                            Value = 32004,
                            Fee = new FeeDb(){ Value = 36 }
                        },
                        new ZipCodeDb()
                        {
                            Value = 32006,
                            Fee = new FeeDb(){ Value = 58 }
                        },
                        new ZipCodeDb()
                        {
                            Value = 32007,
                            Fee = new FeeDb(){ Value = 64 }
                        },
                        new ZipCodeDb()
                        {
                            Value = 32008,
                            Fee = new FeeDb(){ Value = 98 }
                        },
                        new ZipCodeDb()
                        {
                            Value = 32009,
                            Fee = new FeeDb(){ Value = 115 }
                        }
                    }
                },
                new StateDb()
                {
                    Abbreviation = "NY",
                    Name = "New York",
                    VehicleFees = new List<VehicleFeeDb>()
                    {
                        new VehicleFeeDb()
                        {
                            IsAvailable = true,
                            Koefficient = 0.9,
                            Remarks = null,
                            Type = VehicleTypes.Passenger
                        },
                        new VehicleFeeDb()
                        {
                            IsAvailable = true,
                            Koefficient = 1.5,
                            Remarks = null,
                            Type = VehicleTypes.Truck
                        },
                        new VehicleFeeDb()
                        {
                            IsAvailable = true,
                            Koefficient = 1.5,
                            Remarks = null,
                            Type = VehicleTypes.Trailer
                        }
                    },
                    ZipCodes = new List<ZipCodeDb>()
                    {
                        new ZipCodeDb()
                        {
                            Value = 10001,
                            Fee = new FeeDb(){ Value = 80 }
                        },
                        new ZipCodeDb()
                        {
                            Value = 10002,
                            Fee = new FeeDb(){ Value = 140 }
                        },
                        new ZipCodeDb()
                        {
                            Value = 10003,
                            Fee = new FeeDb(){ Value = 70 }
                        },
                        new ZipCodeDb()
                        {
                            Value = 10004,
                            Fee = new FeeDb(){ Value = 84 }
                        },
                        new ZipCodeDb()
                        {
                            Value = 10005,
                            Fee = new FeeDb(){ Value = 96 }
                        }
                    }
                }
            };

            _db.States.AddRange(statesDb);
            await _db.SaveChangesAsync();
        }
    }
}
