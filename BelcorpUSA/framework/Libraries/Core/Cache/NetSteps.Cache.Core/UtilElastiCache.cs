using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ServiceStack.Redis;
using System.Configuration;
using ServiceStack.Redis.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace NetSteps.Core.Cache
{
    /// <summary>
    /// Clase para usar el componente Redis
    /// </summary>
    public class UtilElastiCache
    {
        #region Metodos Privados
        /// <summary>
        /// Método que serializa un objeto de tipo genérico a un arreglo de bytes
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        private byte[] SerializarAByte<T>(T value)
        {
            byte[] data = null;

            using (MemoryStream m = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(m, value);
                data = m.ToArray();
            }

            return data;
        }

        /// <summary>
        /// Método que deserializa a través de un arreglo de bytes
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        private T DeserializarDByte<T>(byte[] data)
        {
            //if (data.Equals(null))
            //    return null;
            T valueAux;
            using (MemoryStream n = new MemoryStream(data))
            {
                var formatter = new BinaryFormatter();
                valueAux = (T)formatter.Deserialize(n);
            }
            return valueAux;
        }
        #endregion

        #region Metodos Públicos
        /// <summary>
        /// Método que permite guardar un registro en caché sin tiempo definido
        /// </summary>
        /// <typeparam name="K">Tipo de objeto de la llave a guardar</typeparam>
        /// <typeparam name="T">Tipo de objeto del valor a guardar</typeparam>
        /// <param name="key">Llave a guardar</param>
        /// <param name="value">Valor a guardar</param>
        /// <returns></returns>
        public bool GuardarCache<K, T>(K key, T value)
        {
            bool agrego = false;

            using (RedisClient redisClient = new RedisClient(ConfigurationManager.AppSettings["hostElastiCache"],
                     Convert.ToInt32(ConfigurationManager.AppSettings["portElastiCache"])))
            {
                IRedisTypedClient<byte[]> objectCacheRedis = redisClient.As<byte[]>();
                //TimeSpan time = new TimeSpan(0, 10, 0);
                byte[] data = SerializarAByte(value);
                string keyStr = Newtonsoft.Json.JsonConvert.SerializeObject(key);
                //
                string tipoKey = key.GetType().FullName;
                string tipoLlave = value.GetType().FullName;
                keyStr = tipoKey + tipoLlave + keyStr;
                //
                objectCacheRedis.SetEntry(keyStr, data);
                agrego = true;
            }

            return agrego;
        }

        /// <summary>
        /// Méotodo que permite guardar un registro en caché con tiempo definido
        /// </summary>
        /// <typeparam name="K">Tipo de objeto de la llave a guardar</typeparam>
        /// <typeparam name="T">Tipo de objeto del valor a guardar</typeparam>
        /// <param name="key">Llave a guardar</param>
        /// <param name="value">Valor a guardar</param>
        /// <param name="time">Tiempo de duración del valor en caché</param>
        /// <returns></returns>
        public bool GuardarCacheTime<K, T>(K key, T value, TimeSpan time)
        {
            bool agrego = false;

            using (RedisClient redisClient = new RedisClient(ConfigurationManager.AppSettings["hostElastiCache"],
                     Convert.ToInt32(ConfigurationManager.AppSettings["portElastiCache"])))
            {
                IRedisTypedClient<byte[]> objectCacheRedis = redisClient.As<byte[]>();
                //TimeSpan time = new TimeSpan(0, 10, 0);
                byte[] data = SerializarAByte(value);
                string keyStr = Newtonsoft.Json.JsonConvert.SerializeObject(key);
                //
                string tipoKey = key.GetType().FullName;
                string tipoLlave = value.GetType().FullName;
                keyStr = tipoKey + tipoLlave + keyStr;
                //
                objectCacheRedis.SetEntry(keyStr, data, time);
                agrego = true;
            }

            return agrego;
        }

        /// <summary>
        /// Método que permite leer un registro de caché
        /// </summary>
        /// <typeparam name="K">Tipo de objeto de la llave a leer</typeparam>
        /// <typeparam name="T">Tipo de objeto del valor a recuperar</typeparam>
        /// <param name="key">Llave consultada</param>
        /// <param name="value">Valor encontrado en caché</param>
        /// <returns>Devuelve el objeto guardado en caché</returns>
        public bool LeerCache<K, T>(K key, out T value)
        {
            bool encontro = false;
            using (RedisClient redisClient = new RedisClient(ConfigurationManager.AppSettings["hostElastiCache"],
                     Convert.ToInt32(ConfigurationManager.AppSettings["portElastiCache"])))
            {
                IRedisTypedClient<byte[]> objectCacheRedis = redisClient.As<byte[]>();

                string keyStr = Newtonsoft.Json.JsonConvert.SerializeObject(key);
                //
                string tipoKey = key.GetType().FullName;
                string tipoLlave = typeof(T).FullName;
                keyStr = tipoKey + tipoLlave + keyStr;
                //
                byte[] data = objectCacheRedis.GetValue(keyStr);
                if (data != null)
                {
                    value = DeserializarDByte<T>(data);
                    encontro = true;
                }
                else
                    value = default(T);
            }
            return encontro;
        }
        #endregion
    }
}
