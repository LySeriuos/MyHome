using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using My_Home.Models;

namespace My_Home
{
    public static class Data
    {
        /// <summary>
        /// Save the list to the local memory
        /// </summary>
        /// <param name="users">All users list</param>
        /// <param name="path">location destination where list will be saved</param>
        public static void SaveUsersListToXml(List<UserProfile> users, string path)
        {
            //    var path = @"C:\Users\shiranco.DESKTOP-HRN41TE\Documents\temp\UserQuestionsAndAnswers.xml";
            XmlSerializer XmlSerializer = new XmlSerializer(typeof(List<UserProfile>));
            using (FileStream file = File.Create(path))
            {
                XmlSerializer.Serialize(file, users);
            }
        }

        /// <summary>
        /// Getting saved List 
        /// </summary>
        /// <param name="path">saved list location destination</param>
        /// <returns>Saved List</returns>
        public static List<UserProfile> GetUsersListFromXml(string path)
        {
            List<UserProfile> usersList = new();
            if (File.Exists(path))
            {
                XmlSerializer XmlSerializer = new XmlSerializer(typeof(List<UserProfile>));
                using (FileStream file = File.OpenRead(path))
                {
                    usersList = XmlSerializer.Deserialize(file) as List<UserProfile>;
                }
            }
            return usersList;
        }
    }
}
