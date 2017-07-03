<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<style type="text/css">
    .pag_btn, .pag_btn_des, .pag_num
    {
        font-size:x-small;
        color: #dff3ff;        
        font-family:Arial;
    }
    
    .pag_btn, .pag_btn_des
    {
        padding: 0px 1px;
        cursor: pointer;
    }
    
    .pag_num
    {
        padding: 0px 10px;   
        color: #ffffff;  
        font-weight:bold;
    }
</style>
<script src="//ajax.googleapis.com/ajax/libs/jquery/1.7.2/jquery.min.js"></script>
<script type="text/javascript">
    function updateParent(ctl) {

        try {
            window.opener.HandlePopupResult(ctl.innerText);
        }
        catch (err) { }
        window.close();
        return false;
    }    
</script>

<script type="text/javascript">
    Paginador = function (divPaginador, tabla, tamPagina) {
        this.miDiv = divPaginador; //un DIV donde irán controles de paginación
        this.tabla = tabla;           //la tabla a paginar
        this.tamPagina = tamPagina; //el tamaño de la página (filas por página)
        this.pagActual = 1;         //asumiendo que se parte en página 1
        this.paginas = Math.floor((this.tabla.rows.length - 1) / this.tamPagina); //¿?

        this.SetPagina = function (num) {
            if (num < 0 || num > this.paginas)
                return;

            this.pagActual = num;
            var min = 1 + (this.pagActual - 1) * this.tamPagina;
            var max = min + this.tamPagina - 1;

            for (var i = 1; i < this.tabla.rows.length; i++) {
                if (i < min || i > max)
                    this.tabla.rows[i].style.display = 'none';
                else
                    this.tabla.rows[i].style.display = '';
            }
            this.miDiv.firstChild.rows[0].cells[1].innerHTML = this.pagActual;
        }

        this.Mostrar = function () {
            //Crear la tabla
            var tblPaginador = document.createElement('table');
            tblPaginador.className = "PaginationContainer";

            //Agregar una fila a la tabla
            var fil = tblPaginador.insertRow(tblPaginador.rows.length);
            fil.className = "Bar";

            //Ahora, agregar las celdas que serán los controles
            var ant = fil.insertCell(fil.cells.length);
            ant.innerHTML = '<a><< Anterior</a>';
            ant.className = 'pag_btn'; //con eso le asigno un estilo
            var self = this;
            ant.onclick = function () {
                if (self.pagActual == 1)
                    return;
                $(".pag_num").html(eval(self.pagActual-1) + ' de ' + self.paginas);
                self.SetPagina(self.pagActual - 1);                
            }

            var num = fil.insertCell(fil.cells.length);
            num.innerHTML = this.pagActual + ' de ' + eval(this.paginas+1); //en rigor, aún no se el número de la página
            num.className = 'pag_num';

            var sig = fil.insertCell(fil.cells.length);
            sig.innerHTML = '<a>Próximo >></a>';
            sig.className = 'pag_btn_des';
            sig.onclick = function () {
                if (self.pagActual == self.paginas)
                    return;
                $(".pag_num").html(eval(self.pagActual + 1) + ' de ' + self.paginas);
                self.SetPagina(self.pagActual + 1);               
            }

            //Como ya tengo mi tabla, puedo agregarla al DIV de los controles
            this.miDiv.appendChild(tblPaginador);

            //¿y esto por qué?
            if (this.tabla.rows.length - 1 > this.paginas * this.tamPagina)
                this.paginas = this.paginas + 1;

            this.SetPagina(this.pagActual);
        }
    }
</script>
<!DOCTYPE html>
<html>
<head runat="server">
    <title>SearchTh</title>
    <style type="text/css">
        
    </style>
</head>
<body>
    <link rel="stylesheet" type="text/css" href="<%= ResolveUrl("~/Resource/Content/CSS/main.css") %>" />
    <div>
        <%--   <span>cuenta id: </span>
        <%: ViewData["AccountId"]%>
        <br />
        <span>Dato a buscar:</span>
        <%: ViewData["Texto"]%>--%>
    </div>
    <br />
    <div>
        <table style="background-color: White" class="DataGrid ClickableDataGrid" id="tblDatos"
            width="100%" cellspacing="0">
            <thead>
                <tr class="GridColHead UI-bg UI-header">
                    <%foreach (System.Data.DataColumn col in (ViewData["DataSe"] as System.Data.DataTable).Columns)
                      { %>
                    <th class="sort" style="font-size: small">
                        <%=col.Caption %>
                    </th>
                    <%} %>
                </tr>
            </thead>
            <tbody>
                <% int p = 1; foreach (System.Data.DataRow row in (ViewData["DataSe"] as System.Data.DataTable).Rows)
                   {
                       int c = 0;  %>
                <tr class="GridColHead UI-bg UI-header">
                    <% foreach (var cell in row.ItemArray)
                       {

                           string cssc = "";
                           if ((p % 2) == 0)
                               cssc = "ALT";%>
                    <td class="<%=cssc %>" style="font-size: small">
                        <%  if (c == 0)
                            {%>
                        <a id="lnk" href="#" onclick="updateParent(this);return false;">
                            <label>
                                <%=cell.ToString()%></label>
                        </a>
                        <%}
                            else
                            {%>
                        <label class="FLabel">
                            <%=cell.ToString()%>
                            </label>
                        <%} c = 1;%>
                    </td>
                    <% } p = p + 1; %>
                </tr>
                <%} %>
            </tbody>
        </table>
        <div id="paginador" class="UI-mainBg Pagination">
            <script type="text/javascript">
                var p = new Paginador(
                document.getElementById('paginador'),
                document.getElementById('tblDatos'), 15);
                p.Mostrar();
            </script>
        </div>
    </div>
</body>
</html>
