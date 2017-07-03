using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Runtime.Serialization;

namespace NetSteps.Silverlight
{
    public static class IsolatedStorage
    {
        // TODO: Add in compression for better utilization of limited client side storage. - JHE

        private static long _minimumIsolatedStorageSize = FileSize.FromMegaBytes(300);
        public static long MinimumIsolatedStorageSize
        {
            get
            {
                return _minimumIsolatedStorageSize;
            }
            set
            {
                _minimumIsolatedStorageSize = value;
            }
        }


        /// <summary>
        /// Saves a key value pair.
        /// </summary>
        /// <param name="key">The key used to retrieve the value.</param>
        /// <param name="value">The value to be stored.</param>
        /// <exception cref="IOException">If unable to write to file.</exception>
        public static void SaveSetting(string key, object value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            string fileName = key + ".txt";
            using (IsolatedStorageFile storageFile = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (value == null || value.ToString().Trim().Length == 0)
                {
                    if (storageFile.FileExists(fileName))
                        storageFile.DeleteFile(fileName);
                }
                else
                {
                    using (IsolatedStorageFileStream isoStream = new IsolatedStorageFileStream(fileName, FileMode.Create, storageFile))
                    {
                        using (StreamWriter writer = new StreamWriter(isoStream))
                        {
                            writer.Write(value);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Tries the get a setting identified by the specified key.
        /// </summary>
        /// <param name="key">The key identifier.</param>
        /// <param name="setting">The setting value.</param>
        /// <returns><code>true</code> if the value was successfully retrieved; 
        /// <code>false</code> otherwise.</returns>
        /// <exception cref="ArgumentNullException">If key is null.</exception>
        public static bool TryGetSetting(string key, out object setting)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            try
            {
                setting = GetSetting(key);
            }
            catch (Exception ex)
            {
                setting = null;
                return false;
            }
            return true;
        }

        public static T GetSetting<T>(string key, T defaultValue)
        {
            if (key == null)
                throw new ArgumentNullException("key");

            Type objectType = typeof(T);
            T result = default(T);
            try
            {
                object value = GetSetting(key);

                object obj = (T)result;
                if (obj is Enum)
                    result = (T)Enum.Parse(objectType, value.ToString(), true);
                else
                    result = (T)Convert.ChangeType(value.ToString(), objectType, null);

                return result;
            }
            catch (Exception ex)
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Gets the setting with the specified key.
        /// </summary>
        /// <param name="key">The key used to identify the setting.</param>
        /// <returns></returns>
        /// <exception cref="IOException">If unable to write to file.</exception>
        /// <exception cref="ArgumentNullException">If key is null.</exception>
        public static object GetSetting(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            string fileName = key + ".txt";

            using (IsolatedStorageFile storageFile = IsolatedStorageFile.GetUserStoreForApplication())
            {
                //if (!storageFile.FileExists(fileName))
                //	return null;

                using (IsolatedStorageFileStream isoStream = new IsolatedStorageFileStream(fileName, FileMode.Open, storageFile))
                {
                    using (StreamReader reader = new StreamReader(isoStream))
                    {
                        /* Read the to the end of the file. */
                        String storedValue = reader.ReadToEnd();
                        return storedValue;
                    }
                }
            }
        }

        /// <summary>
        /// Serializes the specified <code>object</code> to file.
        /// </summary>
        /// <param name="obj">The <code>object</code> to serialize.</param>
        public static void Serialize(object obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }
            string fileName = obj.GetType().FullName + ".txt";
            using (IsolatedStorageFile storageFile
                = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (IsolatedStorageFileStream isoStream
                    = new IsolatedStorageFileStream(fileName, FileMode.Create, storageFile))
                {
                    /* We need another serializer...we can't use this. 
                     * We will update this in Silverlight 2.0 */
                    //					JavaScriptSerializer serializer = new JavaScriptSerializer();
                    //
                    //					using (StreamWriter writer = new StreamWriter(isoStream))
                    //					{
                    //						/* This doesn't work. It's Critical but we require SafeCritical. */
                    //						string serializedInstance = serializer.Serialize(obj);
                    //						writer.Write(serializedInstance);
                    //					}
                }
            }
        }

        /// <summary>
        /// Tries to deserialize an object specified by the type name.
        /// </summary>
        /// <param name="instanceType">Type of the instance to deserialize.</param>
        /// <param name="obj">The object result.</param>
        /// <returns><code>true</code> if desirialization succeeded; 
        /// <code>false</code> otherwise.</returns>
        public static bool TryDeserialize(Type instanceType, out object obj)
        {
            try
            {
                obj = Deserialize(instanceType);
            }
            catch (Exception)
            {
                obj = null;
                return false;
            }
            return true;
        }

        /// <summary>
        /// Deserializes the specified instance type.
        /// </summary>
        /// <param name="instanceType">Type of the instance to desierialize.</param>
        /// <returns>The desierialized instance.</returns>
        public static object Deserialize(Type instanceType)
        {
            string fileName = instanceType.FullName + ".txt";

            using (IsolatedStorageFile storageFile
                = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (IsolatedStorageFileStream isoStream
                    = new IsolatedStorageFileStream(fileName, FileMode.Open, storageFile))
                {
                    using (StreamReader reader = new StreamReader(isoStream))
                    {
                        /* Read to the end of the file. */
                        String storedValue = reader.ReadToEnd();
                        if (string.IsNullOrEmpty(storedValue))
                        {
                            return false;
                        }

                        //						JavaScriptSerializer serializer = new JavaScriptSerializer();
                        //						return serializer.DeserializeObject(storedValue);
                        throw new NotImplementedException();
                    }
                }
            }
        }


        #region Serialize and store in IS with DataContractSerializer
        // http://www.bbits.co.uk/blog/archive/2008/11/27/serializing-objects-to-isolated-storage-in-silverlight-2.aspx
        public static void SaveData<T>(T dataToSave, string fileName) where T : class
        {
            try
            {
                using (IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForSite())
                {
                    IsolatedStorageFileStream stream = store.CreateFile(fileName);
                    DataContractSerializer serializer = new DataContractSerializer(typeof(T));
                    serializer.WriteObject(stream, dataToSave);
                    stream.Close();
                }
            }
            catch
            {
                try
                {
                    IsolatedStorage.DeleteFile(fileName);   // Remove file on failure - JHE
                }
                catch { }
            }
        }

        public static T LoadData<T>(string fileName) where T : class
        {
            using (IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForSite())
            {
                try
                {
                    IsolatedStorageFileStream stream = new IsolatedStorageFileStream(fileName, FileMode.Open, store);
                    DataContractSerializer serializer = new DataContractSerializer(typeof(T));
                    T p = (T)serializer.ReadObject(stream);
                    stream.Close();
                    return p;
                }
                catch (SerializationException ex)
                {
                    return null;
                }
                catch
                {
                    return null;
                }
            }
        }

        public static bool ContainsFile(string fileName)
        {
            try
            {
                using (IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForSite())
                {
                    return store.FileExists(fileName);
                }
            }
            catch
            {
                throw new Exception(string.Format("IsolatedStorage: Error calling 'ContainsFile' for filename: {0}", fileName));
            }
        }

        public static void DeleteFile(string fileName)
        {
            using (IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForSite())
            {
                if (store.FileExists(fileName))
                    store.DeleteFile(fileName);
            }
        }
        #endregion

        public static long StoreSize
        {
            get
            {
                using (IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForSite())
                {
                    return store.Quota;
                }
            }
        }

        public static long FreeSpace
        {
            get
            {
                using (IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForSite())
                {
                    return store.AvailableFreeSpace;
                }
            }
        }

        public static void AllocateStorage()
        {
            AllocateStorage(10737418240);
        }
        public static void AllocateStorage(long newQuota)
        {
            using (IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForSite())
            {
                if (store.Quota < newQuota)
                {
                    store.IncreaseQuotaTo(newQuota);
                }
            }
        }
    }
}
