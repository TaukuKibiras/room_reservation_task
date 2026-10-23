using Microsoft.EntityFrameworkCore;
using Reservation.ServiceLibrary;
using Reservation.ServiceLibrary.Entities;

namespace ReactApp1.Server
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(AppDbContext context)
        {
            if (!await context.User.AnyAsync())
            {
                context.User.AddRange(
                    new UserEntity
                    {
                        Name = "Jonas",
                        Surname = "Jonaitis",
                        Reservations = 0
                    },
                    new UserEntity
                    {
                        Name = "Ona",
                        Surname = "Onaityte",
                        Reservations = 0
                    },
                    new UserEntity
                    {
                        Name = "Petras",
                        Surname = "Petraitis",
                        Reservations = 0
                    },
                    new UserEntity
                    {
                        Name = "Ieva",
                        Surname = "Kazlauskaite",
                        Reservations = 0
                    }
                );
            }

            if (!await context.MeetingRoom.AnyAsync())
            {
                context.MeetingRoom.AddRange(
                    new MeetingRoomEntity
                    {
                        Name = "Conference Room A",
                        Capacity = 12,
                        Location = "1st floor",
                        Equipment = "Projector, Whiteboard",
                        IsActive = true
                    },
                    new MeetingRoomEntity
                    {
                        Name = "Meeting Room B",
                        Capacity = 6,
                        Location = "2nd floor",
                        Equipment = "TV, Whiteboard",
                        IsActive = true
                    },
                    new MeetingRoomEntity
                    {
                        Name = "Creative Room",
                        Capacity = 10,
                        Location = "3rd floor",
                        Equipment = "TV, Video conferencing",
                        IsActive = true
                    },
                    new MeetingRoomEntity
                    {
                        Name = "Small Meeting Room",
                        Capacity = 4,
                        Location = "1st floor",
                        Equipment = "Whiteboard",
                        IsActive = true
                    }
                );
            }

            await context.SaveChangesAsync();
        }
    }
}