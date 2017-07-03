 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;


namespace NetSteps.Data.Entities
{
    public static class Extension
    {
        public enum TipoOrden
        {
            Asc,
            Desc

        }
        /// <summary>
        /// funcion creada por salcedo vila gaylussac
        /// </summary>
        /// <typeparam name="T">el tipo de dato el  cual se quiere paginar</typeparam>
        /// <param name="ListaPaginar">la lista que se va paginar </param>
        /// <param name="pagina">pagina actual </param>
        /// <param name="FilasXPagina">la cantidad de registros por pagina </param>
        /// <returns></returns>
        public static IEnumerable<T> PaginarLista<T>(this IEnumerable<T> ListaPaginar, int pagina, int FilasXPagina)
        {
            var index = 0;
            int lismSup = (pagina * FilasXPagina) - 1;
            int limInf = lismSup - (FilasXPagina - 1);
            foreach (var el in ListaPaginar)
            {
                if (index >= limInf && index <= lismSup) yield return el;
                index += 1;
            }
        }

        public static int cantidadPaginas(this int totalRegistrosBD, int filasXpagina)
        {
            int paginasSobra = 0;
            int numeroPaginas = 0;
            int sobra = 0;
            paginasSobra = paginasSobra = totalRegistrosBD % filasXpagina;
            if (totalRegistrosBD > filasXpagina)
            {
                numeroPaginas = totalRegistrosBD / filasXpagina;
            }
            else
            {
                numeroPaginas = 1;
            }
            sobra = (numeroPaginas * filasXpagina) + paginasSobra;

            if (Convert.ToInt32(paginasSobra) > 0 && sobra == totalRegistrosBD)
            {
                numeroPaginas += 1;
            }
            return numeroPaginas;
        }

        /// <summary>
        ///Metodo creado por salcedo vila gaylussac  Para filtrar por culquier campo
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ListaFiltrar"> lista que se va  paginar </param>
        /// <param name="filtro"></param>
        /// <returns></returns>
        public static IEnumerable<T> FiltroDinamico<T>(this IEnumerable<T> ListaFiltrar, IEnumerable<NombreColumnas> filtro)
        {
            Boolean esIgual = true;

            foreach (var Obj in ListaFiltrar)
            {
                esIgual = true;
                foreach (var ocl in filtro)
                {
                    var valorCampo = (Obj) == null ? ocl.ValorColumna : ObtenerValor<T>(ocl.NombreColumna.Trim(), Obj);


                    if (!valorCampo.ToString().ToUpper().Contains(ocl.ValorColumna.ToUpper()))
                    {
                        esIgual = false;
                        break;
                    }
                }

                if (esIgual) yield return Obj;
            }
        }

        static String ObtenerValor<T>(string Key, T tipo)
        {
            ParameterExpression p = Expression.Parameter(typeof(T), "a");
            Expression Body = Expression.PropertyOrField(p, Key);
            PropertyInfo prop = typeof(T).GetProperty(Key);

            String Ty = prop.PropertyType.Name.ToString();
            switch (Ty)
            {
                case "String":
                    Func<T, string> FunString = Expression.Lambda<Func<T, string>>(Body, new ParameterExpression[] { p }).Compile();
                    return FunString(tipo).ToString();
                    break;
                case "Decimal":
                    Func<T, decimal> funcDecimal = Expression.Lambda<Func<T, decimal>>(Body, new ParameterExpression[] { p }).Compile();
                    return funcDecimal(tipo).ToString();
                    break;
                case "Int32":
                    Func<T, int> funcInt = Expression.Lambda<Func<T, int>>(Body, new ParameterExpression[] { p }).Compile();
                    return funcInt(tipo).ToString();
                    break;
                case "DateTime":
                    Func<T, DateTime> funcdate = Expression.Lambda<Func<T, DateTime>>(Body, new ParameterExpression[] { p }).Compile();
                    return funcdate(tipo).ToString();
                    break;
                case "Double":
                    Func<T, Double> funcDouble = Expression.Lambda<Func<T, Double>>(Body, new ParameterExpression[] { p }).Compile();
                    return funcDouble(tipo).ToString();
                    break;
                case "Single":
                    Func<T, float> funcFloat = Expression.Lambda<Func<T, float>>(Body, new ParameterExpression[] { p }).Compile();
                    return funcFloat(tipo).ToString();
                    break;
            }
            return "";
        }

        /// <summary>
        /// FILTRAR 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ListaOrdenar"></param>
        /// <param name="NombreCampo"></param>
        /// <param name="tp"></param>
        /// <returns></returns>
        public static IEnumerable<T> OrdenarPorCampo<T>(this  IEnumerable<T> ListaOrdenar, string NombreCampo, NetSteps.Common.Constants.SortDirection tp)
        {
            Type ot = typeof(T);
            try
            {
                if (NombreCampo.Trim() == "") throw new Exception("El nombre del campos esta vacio");

                ParameterExpression Key = Expression.Parameter(ot, "Key");
                Expression func = Expression.PropertyOrField(Key, NombreCampo);

                PropertyInfo proInfo = typeof(T).GetProperty(NombreCampo.Trim());
                String Ty = proInfo.PropertyType.Name.ToString();

                switch (tp)
                {
                    case NetSteps.Common.Constants.SortDirection.Ascending:
                        switch (Ty)
                        {
                            case "String":
                                Func<T, string> FunString = Expression.Lambda<Func<T, string>>(func, new ParameterExpression[] { Key }).Compile();
                                return ListaOrdenar.AsQueryable().OrderBy(FunString);
                                break;
                            case "Decimal":
                                Func<T, decimal> funcDecimal = Expression.Lambda<Func<T, decimal>>(func, new ParameterExpression[] { Key }).Compile();
                                return ListaOrdenar.AsQueryable().OrderBy(funcDecimal);
                                break;
                            case "Int32":
                                Func<T, int> funcInt = Expression.Lambda<Func<T, int>>(func, new ParameterExpression[] { Key }).Compile();
                                return ListaOrdenar.AsQueryable().OrderBy(funcInt);
                                break;
                            case "DateTime":
                                Func<T, DateTime> funcdate = Expression.Lambda<Func<T, DateTime>>(func, new ParameterExpression[] { Key }).Compile();
                                return ListaOrdenar.AsQueryable().OrderBy(funcdate);
                                break;
                            case "Double":
                                Func<T, Double> funcDouble = Expression.Lambda<Func<T, Double>>(func, new ParameterExpression[] { Key }).Compile();
                                return ListaOrdenar.AsQueryable().OrderBy(funcDouble);
                                break;
                            case "Single":
                                Func<T, float> funcFloat = Expression.Lambda<Func<T, float>>(func, new ParameterExpression[] { Key }).Compile();
                                return ListaOrdenar.AsQueryable().OrderBy(funcFloat);
                                break;
                        }
                        break;
                    default:
                        switch (Ty)
                        {
                            case "String":
                                Func<T, string> FunString = Expression.Lambda<Func<T, string>>(func, new ParameterExpression[] { Key }).Compile();
                                return ListaOrdenar.AsQueryable().OrderByDescending(FunString);
                                break;
                            case "Decimal":
                                Func<T, decimal> funcDecimal = Expression.Lambda<Func<T, decimal>>(func, new ParameterExpression[] { Key }).Compile();
                                return ListaOrdenar.AsQueryable().OrderByDescending(funcDecimal);
                                break;
                            case "Int32":
                                Func<T, int> funcInt = Expression.Lambda<Func<T, int>>(func, new ParameterExpression[] { Key }).Compile();
                                return ListaOrdenar.AsQueryable().OrderByDescending(funcInt);
                                break;
                            case "DateTime":
                                Func<T, DateTime> funcdate = Expression.Lambda<Func<T, DateTime>>(func, new ParameterExpression[] { Key }).Compile();
                                return ListaOrdenar.AsQueryable().OrderByDescending(funcdate);
                                break;
                            case "Double":
                                Func<T, Double> funcDouble = Expression.Lambda<Func<T, Double>>(func, new ParameterExpression[] { Key }).Compile();
                                return ListaOrdenar.AsQueryable().OrderByDescending(funcDouble);
                                break;
                            case "Single":
                                Func<T, float> funcFloat = Expression.Lambda<Func<T, float>>(func, new ParameterExpression[] { Key }).Compile();
                                return ListaOrdenar.AsQueryable().OrderByDescending(funcFloat);
                                break;
                        }
                        break;
                }
                return ListaOrdenar.AsQueryable();


            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
    }

    public class NombreColumnas
    {
        public string ValorColumna { get; set; }
        public string NombreColumna { get; set; }
    }
    public class paginar<T>
    {
        private int cantidadFilas;
        private int paginaActual;
        private int filasXPagina;
        public int totalpages { get; private set; }
        public IEnumerable<T> lista;

        public int CantidadFila { get { return cantidadFilas; } }
        public paginar(IEnumerable<T> listaDatos)
        {
            lista = listaDatos ?? new List<T>() { };
        }

        public paginar(int PaginaActual, int FilasXPagina, IEnumerable<T> listaDatos)
        {
            this.paginaActual = PaginaActual;
            this.filasXPagina = FilasXPagina;
            lista = listaDatos ?? new List<T>() { };
            this.totalpages = (lista.Count()).cantidadPaginas(this.filasXPagina);
        }
        public paginar<T> Filtrar(IEnumerable<NombreColumnas> filtro)
        {
            lista = lista.FiltroDinamico(filtro);
            cantidadFilas = lista.Count();
            return this;
        }
        public paginar<T> Ordenar(string campo, NetSteps.Common.Constants.SortDirection  tipoOrden)
        {
            lista = lista.OrdenarPorCampo(campo, tipoOrden);
            return this;

        }
        public paginar<T> Paginar()
        {
            lista = lista.PaginarLista(this.paginaActual, this.filasXPagina);
            return this;
        }

        public paginar(int PaginaActual, int FilasXPagina, IEnumerable<T> listaDatos, IEnumerable<NombreColumnas> filtro, string campo, NetSteps.Common.Constants.SortDirection tipoOrden)
        {
            lista = lista.FiltroDinamico(filtro);
            lista = lista.OrdenarPorCampo(campo, tipoOrden);
            Paginar();
        }
    }
     
}
