using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Globalization;
using VK_Module.Scripts;

namespace VK_Module.Databases.AdvertisementsDbModel
{
    public class AdvertisementsDbModel : DbConnectionInitializer, IAdvertisementsRepository<Advertisement>
    {
        public AdvertisementsDbModel(string dbPath) : base(dbPath) { }

        public void AddAdvertisement(Advertisement adExemplar)
        {
            using (connection)
            {
                try
                {
                    connection.Open();
                SqliteCommand command = connection.CreateCommand();

                command.CommandText = @"INSERT INTO Advertisements (id, link, text, poster, date, imageUrls, estateType, roomsCount, state)
                                 VALUES (@id, @link, @text, @poster, @date, @imageUrls, @estateType, @roomsCount, @state)";

                command.Parameters.AddWithValue("@id", adExemplar.Id);
                command.Parameters.AddWithValue("@link", adExemplar.Link);
                command.Parameters.AddWithValue("@text", adExemplar.Description);
                command.Parameters.AddWithValue("@poster", adExemplar.Poster);
                command.Parameters.AddWithValue("@date", adExemplar.Date);
                command.Parameters.AddWithValue("@imageUrls", adExemplar.ImageLinks);
                command.Parameters.AddWithValue("@estateType", adExemplar.HousingType);
                command.Parameters.AddWithValue("@roomsCount", adExemplar.RoomCount);
                command.Parameters.AddWithValue("@state", "Загружено");

                command.ExecuteNonQuery();
            }
                catch
                {

            }
        }
        }

        public Advertisement GetAdvertisementById(Advertisement adExemplar)
        {
            Advertisement ad = new Advertisement();
            using (connection)
            {
                connection.Open();
                SqliteCommand command = connection.CreateCommand();
                command.CommandText = @$"SELECT id, link, text, poster, date, imageUrls, estateType, roomsCount, state 
                                         FROM Advertisements WHERE id={adExemplar.Id}";
                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ad.Id = reader.GetString(0);
                        ad.Link = reader.GetString(1);
                        ad.Description = reader.GetString(2);
                        ad.Poster = reader.GetString(3);
                        ad.Date = reader.GetDateTime(4);
                        ad.ImageLinks = reader.GetString(5);
                        ad.HousingType = reader.GetString(6);
                        ad.RoomCount = reader.GetString(7);
                        ad.State = reader.GetString(8);
                    }
                }               
            }
            return ad;
        }

        public List<Advertisement> GetAllAdvertisements()
        {
            List<Advertisement> advertisements = new List<Advertisement>();
            using (connection)
            {
                connection.Open();
                SqliteCommand command = connection.CreateCommand();
                command.CommandText = @$"SELECT id, link, text, poster, date, imageUrls, estateType, roomsCount, state FROM Advertisements";
                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        advertisements.Add(new Advertisement()
                        {
                            Id = reader.GetString(0),
                            Link = reader.GetString(1),
                            Description = reader.GetString(2),
                            Poster = reader.GetString(3),
                            //Date = DateTime.ParseExact(reader.GetString(4).Split(" ")[0], "dd.MM.yyyy", CultureInfo.InvariantCulture),
                            Date = reader.GetDateTime(4),
                            ImageLinks = reader.GetString(5),
                            HousingType = reader.GetString(6),
                            RoomCount = reader.GetString(7),
                            State = reader.GetString(8),
                        });
                    }
                }
            }
            return advertisements;
        }

        public void RemoveAdvertisementById(Advertisement adExemplar)
        {
            using (connection)
            {
                connection.Open();
                SqliteCommand command = connection.CreateCommand();
                command.CommandText = $"DELETE FROM Advertisements WHERE id='{adExemplar.Id}'";                
                command.ExecuteNonQuery();
            }
        }

        public void CleanDatabase()
        {
            using (connection)
            {
                connection.Open();
                SqliteCommand command = connection.CreateCommand();
                command.CommandText = $@"DELETE FROM Advertisements";
                command.ExecuteNonQuery();                
            }
        }

        public void UpdateAdvertisementById(Advertisement adExemplar)
        {
            using (connection)
            {
                connection.Open();
                SqliteCommand command = connection.CreateCommand();
                command.CommandText = @$"UPDATE Advertisements 
                                 SET link = '{adExemplar.Link}',
                                     text = '{adExemplar.Description}',
                                     poster = '{adExemplar.Poster}',
                                     date = '{adExemplar.Date}',
                                     imageUrls = '{adExemplar.ImageLinks}',
                                     estateType = '{adExemplar.HousingType}',
                                     roomsCount = '{adExemplar.RoomCount}',
                                     state = 'Изменено'
                                 WHERE id = '{adExemplar.Id}'";
                command.ExecuteNonQuery();
            }
        }
    }
}
