﻿<%@  Language="C#" MasterPageFile="~/Areas/Sites/Views/Shared/Sites.Master" Inherits="System.Web.Mvc.ViewPage<IList<NetSteps.Data.Entities.Archive>>" %>
<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        //<![CDATA[
        $(function () {
            $('#messageCenter').messageCenter({ iconPath: '/Resource/Content/Images/icon-Alert.png', includeCustomExitIcon: true });
        });
        function showMessage(message, isError) {
            if (isError && message === undefined) {
                message = '<div class="messageText">An error occurred.  Please try again later.</div>';
            }
            $('#messageCenter').messageCenter('clearAllMessages').messageCenter('addMessage', message, !isError ? { iconPath: '../Resource/Content/Images/icon-Success.png'} : undefined);
            $('#messageCenterModal').show();

            if (!isError) {
                setTimeout(hideMessage, 3000);
            }

            if (isError) {
                $('#messageCenterModal').addClass('thisIsNowAnErrorMessage');
            } else {
                $('#messageCenterModal').removeClass('thisIsNowAnErrorMessage');
            }

            $('.errorMessageBubble, .messageCenterMessage').die().live('click', function () {
                hideMessage();
            });
        }
        function hideMessage() {
            $('#messageCenterModal').hide();
        }
	    //]]>
    </script>
    <script type="text/javascript">
        //<![CDATA[
        $('td.CoreContent input.emFileURL').live('click', function () {
            $(this).select();
        }); 
	    //]]>
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BreadCrumbContent" runat="server">
	<a href="<%= ResolveUrl("~/Sites") %>"><%= Html.Term("Sites") %></a> >
	<%= Html.Term("Manage_Design_Images", "Manage Design Images") %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="YellowWidget" runat="server">
</asp:Content>
<asp:Content ContentPlaceHolderID="LeftNav" runat="server">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<div class="SectionHeader">
		<h2>
			<%= Html.Term("Manage_Design_Images", "Manage Design Images")%></h2>
	</div>

    <div class="UI-lightBg brdrAll GridFilters">
        <div class="FL">
            <%= Html.Term("DateRange", "Date Range") %>:<br />
            <input type="text" id="startDate" class="DatePicker TextInput" value="<%= Html.Term("StartDate", "Start Date") %>" />
            to
            <input type="text" id="endDate" class="DatePicker TextInput" value="<%= Html.Term("EndDate", "End Date") %>" />
		</div> 
        <span class="clr"></span>    
    </div>
    <div class="UI-mainBg icon-24 brdrAll brdr1 GridSelectOptions GridUtility" id="paginatedGridOptions">
        <a href="javascript:void(0);" class="clearFiltersButton UI-icon-container"><span class="UI-icon icon-refresh"></span><span>Clear Filters</span></a>
        <span class="ClearAll"></span>
    </div>
    <table width="100%" cellspacing="0" class="DataGrid" id="paginatedGrid">
        <thead>
            <tr class="GridColHead UI-bg UI-header">
                <th id="Image" title="Sort by..." class="sort currentSort Ascending" colspan="1">
                    <a style="float: left;" href="javascript:void(0);">Image</a>
                <span class="IconLink SortColumn" style="float: right;"></span></th>
                <th id="FilePath" title="Sort by..." class="sort" colspan="1">
                    <a style="float: left;" href="javascript:void(0);">File Path</a>
                </th>
                <th id="Th1" title="Sort by..." class="sort" colspan="1">
                    <a style="float: left;" href="javascript:void(0);">Facebook ID Path</a>
                </th>
                <th id="CreateDate" title="Sort by..." class="sort" colspan="1">
                    <a style="float: left;" href="javascript:void(0);">Date Created</a>
                </th>
                <th id="CustomerName" title="Sort by..." class="sort" colspan="1">
                    <a style="float: left;" href="javascript:void(0);">Customer Name</a>
                </th>
                <th id="Email" title="Sort by..." class="sort" colspan="1">
                    <a style="float: left;" href="javascript:void(0);">Email</a>
                </th>				
                <th id="Likes" title="Sort by..." class="sort" colspan="1">
                    <a style="float: left;" href="javascript:void(0);">Likes</a>
                </th>
                <th colspan="1" class="sort" title="Sort by..." id="TotalOrders">
                    <a href="javascript:void(0);" style="float: left;">Total Orders</a>
                </th>
                <th id="LastOrder" title="Sort by..." class="sort" colspan="1">
                    <a href="javascript:void(0);" style="float: left;">Last Completed Order</a>
                </th>
            </tr>
        </thead>
        <tbody>
            <tr class="Alt">
                <td><a href="#"><img width="125" src="http://base.netstepsdemo.com/FileUploads/Logo/encorelogo.png" alt=""></a></td>
                <td><input type="text" value="http://base.netstepsdemo.com/FileUploads/Logo/encorelogo.png" class="emFileURL"></td>
                <td><input type="text" value="http://base.netstepsdemo.com/Novellas/idhere1" class="emFileURL"></td>
                <td>6/22/2011 4:04:00 PM</td>
                <td><a href="#">Abe Abeson</a></td>
                <td>Consultant: <a href="mailTo:">consultant@email.com</a><br>Customer: <a href="mailTo:">abeabeson@hotmail.com</a></td>
                <td>43</td>
                <td>57</td>
                <td>7/7/2010 2:53:00 PM</td>
            </tr>
            <tr class="">
                <td><a href="#"><img width="125" alt="" src="http://test.netsteps.com/FileUploads/CMS/Images/leftColAd.jpg"></a></td>
                <td><input type="text" value="http://test.netsteps.com/FileUploads/CMS/Images/leftColAd.jpg" class="emFileURL"></td>
                <td><input type="text" value="http://base.netstepsdemo.com/Novellas/idhere2" class="emFileURL"></td>
                <td>6/22/2011 4:04:00 PM</td>
                <td><a href="#">Bob Bobson</a></td>
                <td>Consultant: <a href="mailTo:">consultant@email.com</a><br>Customer: <a href="mailTo:">bobbobson12@gmail.com</a></td>
                <td>24</td>
                <td>1</td>
                <td>6/9/2010 2:53:00 PM</td>
            </tr>
        </tbody>
    </table>
    <div style="position: absolute; top: 253.5px; left: 238px; z-index: 1; display: none;" id="ui-datepicker-div" class="ui-datepicker ui-widget ui-widget-content ui-helper-clearfix ui-corner-all">
        <div class="ui-datepicker-header ui-widget-header ui-helper-clearfix ui-corner-all">
            <a class="ui-datepicker-prev ui-corner-all" onclick="DP_jQuery_1321382478403.datepicker._adjustDate('#startDate', -1, 'M');" title="Prev">
                <span class="ui-icon ui-icon-circle-triangle-w">Prev</span></a>
            <a class="ui-datepicker-next ui-corner-all" onclick="DP_jQuery_1321382478403.datepicker._adjustDate('#startDate', +1, 'M');" title="Next">
                <span class="ui-icon ui-icon-circle-triangle-e">Next</span></a>
            <div class="ui-datepicker-title">
            <select class="ui-datepicker-month" onchange="DP_jQuery_1321382478403.datepicker._selectMonthYear('#startDate', this, 'M');" onclick="DP_jQuery_1321382478403.datepicker._clickMonthYear('#startDate');">
                <option value="0">Jan</option>
                <option value="1">Feb</option>
                <option value="2">Mar</option>
                <option value="3">Apr</option>
                <option value="4">May</option>
                <option value="5">Jun</option>
                <option value="6">Jul</option>
                <option value="7">Aug</option>
                <option value="8">Sep</option>
                <option value="9">Oct</option>
                <option value="10" selected="selected">Nov</option>
                <option value="11">Dec</option>
            </select>
            <select class="ui-datepicker-year" onchange="DP_jQuery_1321382478403.datepicker._selectMonthYear('#startDate', this, 'Y');" onclick="DP_jQuery_1321382478403.datepicker._clickMonthYear('#startDate');">
                <option value="1911">1911</option><option value="1912">1912</option><option value="1913">1913</option>
                <option value="1914">1914</option><option value="1915">1915</option><option value="1916">1916</option>
                <option value="1917">1917</option><option value="1918">1918</option><option value="1919">1919</option>
                <option value="1920">1920</option><option value="1921">1921</option><option value="1922">1922</option>
                <option value="1923">1923</option><option value="1924">1924</option><option value="1925">1925</option>
                <option value="1926">1926</option><option value="1927">1927</option><option value="1929">1929</option>
                <option value="1930">1930</option><option value="1931">1931</option><option value="1932">1932</option>
                <option value="1933">1933</option><option value="1934">1934</option><option value="1935">1935</option>
                <option value="1936">1936</option><option value="1937">1937</option><option value="1938">1938</option>
                <option value="1939">1939</option><option value="1940">1940</option><option value="1941">1941</option>
                <option value="1942">1942</option><option value="1943">1943</option><option value="1944">1944</option>
                <option value="1945">1945</option><option value="1946">1946</option><option value="1947">1947</option>
                <option value="1948">1948</option><option value="1949">1949</option><option value="1950">1950</option>
                <option value="1951">1951</option><option value="1952">1952</option><option value="1953">1953</option>
                <option value="1954">1954</option><option value="1955">1955</option><option value="1956">1956</option>
                <option value="1957">1957</option><option value="1958">1958</option><option value="1959">1959</option>
                <option value="1960">1960</option><option value="1961">1961</option>
                <option value="1962">1962</option><option value="1963">1963</option><option value="1964">1964</option>
                <option value="1965">1965</option><option value="1966">1966</option><option value="1967">1967</option>
                <option value="1968">1968</option><option value="1969">1969</option><option value="1970">1970</option>
                <option value="1971">1971</option><option value="1972">1972</option><option value="1973">1973</option>
                <option value="1974">1974</option><option value="1975">1975</option><option value="1976">1976</option>
                <option value="1977">1977</option><option value="1978">1978</option><option value="1979">1979</option>
                <option value="1980">1980</option><option value="1981">1981</option><option value="1982">1982</option>
                <option value="1983">1983</option><option value="1984">1984</option><option value="1985">1985</option>
                <option value="1986">1986</option><option value="1987">1987</option><option value="1988">1988</option>
                <option value="1989">1989</option><option value="1990">1990</option><option value="1991">1991</option>
                <option value="1992">1992</option><option value="1993">1993</option><option value="1994">1994</option>
                <option value="1995">1995</option><option value="1996">1996</option><option value="1997">1997</option>
                <option value="1998">1998</option><option value="1999">1999</option><option value="2000">2000</option>
                <option value="2001">2001</option><option value="2002">2002</option><option value="2003">2003</option>
                <option value="2004">2004</option><option value="2005">2005</option><option value="2006">2006</option>
                <option value="2007">2007</option><option value="2008">2008</option><option value="2009">2009</option>
                <option value="2010">2010</option><option value="2011" selected="selected">2011</option><option value="2012">2012</option>
                <option value="2013">2013</option><option value="2014">2014</option><option value="2015">2015</option>
                <option value="2016">2016</option><option value="2017">2017</option><option value="2018">2018</option>
                <option value="2019">2019</option><option value="2020">2020</option><option value="2021">2021</option>
                <option value="2022">2022</option><option value="2023">2023</option><option value="2024">2024</option>
                <option value="2025">2025</option><option value="2026">2026</option><option value="2027">2027</option>
                <option value="2028">2028</option><option value="2029">2029</option><option value="2030">2030</option>
                <option value="2031">2031</option><option value="2032">2032</option><option value="2033">2033</option>
                <option value="2034">2034</option><option value="2035">2035</option><option value="2036">2036</option>
                <option value="2037">2037</option><option value="2038">2038</option><option value="2039">2039</option>
                <option value="2040">2040</option><option value="2041">2041</option><option value="2042">2042</option>
                <option value="2043">2043</option><option value="2044">2044</option><option value="2045">2045</option>
                <option value="2046">2046</option><option value="2047">2047</option><option value="2048">2048</option>
                <option value="2049">2049</option><option value="2050">2050</option><option value="2051">2051</option>
                <option value="2052">2052</option><option value="2053">2053</option><option value="2054">2054</option>
                <option value="2055">2055</option><option value="2056">2056</option><option value="2057">2057</option>
                <option value="2058">2058</option><option value="2059">2059</option><option value="2060">2060</option>
                <option value="2061">2061</option><option value="2062">2062</option><option value="2063">2063</option>
                <option value="2064">2064</option><option value="2065">2065</option><option value="2066">2066</option>
                <option value="2067">2067</option><option value="2068">2068</option><option value="2069">2069</option>
                <option value="2070">2070</option><option value="2071">2071</option><option value="2072">2072</option>
                <option value="2073">2073</option><option value="2074">2074</option><option value="2075">2075</option>
                <option value="2076">2076</option><option value="2077">2077</option><option value="2078">2078</option>
                <option value="2079">2079</option><option value="2080">2080</option><option value="2081">2081</option>
                <option value="2082">2082</option><option value="2083">2083</option><option value="2084">2084</option>
                <option value="2085">2085</option><option value="2086">2086</option><option value="2087">2087</option>
                <option value="2088">2088</option><option value="2089">2089</option><option value="2090">2090</option>
                <option value="2091">2091</option><option value="2092">2092</option><option value="2093">2093</option>
                <option value="2094">2094</option><option value="2095">2095</option><option value="2096">2096</option>
                <option value="2097">2097</option><option value="2098">2098</option><option value="2099">2099</option>
                <option value="2100">2100</option><option value="2101">2101</option><option value="2102">2102</option>
                <option value="2103">2103</option><option value="2104">2104</option><option value="2105">2105</option>
                <option value="2106">2106</option><option value="2107">2107</option><option value="2108">2108</option>
                <option value="2109">2109</option><option value="2110">2110</option><option value="2111">2111</option>
            </select>
        </div>
        </div>
        <table class="ui-datepicker-calendar">
            <thead>
                <tr>
                    <th class="ui-datepicker-week-end"><span title="Sunday">Su</span></th>
                    <th><span title="Monday">Mo</span></th>
                    <th><span title="Tuesday">Tu</span></th>
                    <th><span title="Wednesday">We</span></th>
                    <th><span title="Thursday">Th</span></th>
                    <th><span title="Friday">Fr</span></th>
                    <th class="ui-datepicker-week-end"><span title="Saturday">Sa</span></th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td class=" ui-datepicker-week-end ui-datepicker-other-month ui-datepicker-unselectable ui-state-disabled">&nbsp;</td>
                    <td class=" ui-datepicker-other-month ui-datepicker-unselectable ui-state-disabled">&nbsp;</td>
                    <td class=" " onclick="DP_jQuery_1321382478403.datepicker._selectDay('#startDate',10,2011, this);return false;"><a class="ui-state-default" href="#">1</a></td>
                    <td class=" " onclick="DP_jQuery_1321382478403.datepicker._selectDay('#startDate',10,2011, this);return false;"><a class="ui-state-default" href="#">2</a></td>
                    <td class=" " onclick="DP_jQuery_1321382478403.datepicker._selectDay('#startDate',10,2011, this);return false;"><a class="ui-state-default" href="#">3</a></td>
                    <td class=" " onclick="DP_jQuery_1321382478403.datepicker._selectDay('#startDate',10,2011, this);return false;"><a class="ui-state-default" href="#">4</a></td>
                    <td class=" ui-datepicker-week-end " onclick="DP_jQuery_1321382478403.datepicker._selectDay('#startDate',10,2011, this);return false;"><a class="ui-state-default" href="#">5</a></td>
                </tr>
                <tr>
                    <td class=" ui-datepicker-week-end " onclick="DP_jQuery_1321382478403.datepicker._selectDay('#startDate',10,2011, this);return false;"><a class="ui-state-default" href="#">6</a></td>
                    <td class=" " onclick="DP_jQuery_1321382478403.datepicker._selectDay('#startDate',10,2011, this);return false;"><a class="ui-state-default" href="#">7</a></td>
                    <td class=" " onclick="DP_jQuery_1321382478403.datepicker._selectDay('#startDate',10,2011, this);return false;"><a class="ui-state-default" href="#">8</a></td>
                    <td class=" " onclick="DP_jQuery_1321382478403.datepicker._selectDay('#startDate',10,2011, this);return false;"><a class="ui-state-default" href="#">9</a></td>
                    <td class=" " onclick="DP_jQuery_1321382478403.datepicker._selectDay('#startDate',10,2011, this);return false;"><a class="ui-state-default" href="#">10</a></td>
                    <td class=" " onclick="DP_jQuery_1321382478403.datepicker._selectDay('#startDate',10,2011, this);return false;"><a class="ui-state-default" href="#">11</a></td>
                    <td class=" ui-datepicker-week-end " onclick="DP_jQuery_1321382478403.datepicker._selectDay('#startDate',10,2011, this);return false;"><a class="ui-state-default" href="#">12</a></td>
                </tr>
                <tr>
                    <td class=" ui-datepicker-week-end " onclick="DP_jQuery_1321382478403.datepicker._selectDay('#startDate',10,2011, this);return false;"><a class="ui-state-default" href="#">13</a></td>
                    <td class=" " onclick="DP_jQuery_1321382478403.datepicker._selectDay('#startDate',10,2011, this);return false;"><a class="ui-state-default" href="#">14</a></td>
                    <td class=" ui-datepicker-days-cell-over  ui-datepicker-current-day ui-datepicker-today" onclick="DP_jQuery_1321382478403.datepicker._selectDay('#startDate',10,2011, this);return false;"><a class="ui-state-default ui-state-highlight ui-state-active" href="#">15</a></td>
                    <td class=" " onclick="DP_jQuery_1321382478403.datepicker._selectDay('#startDate',10,2011, this);return false;"><a class="ui-state-default" href="#">16</a></td>
                    <td class=" " onclick="DP_jQuery_1321382478403.datepicker._selectDay('#startDate',10,2011, this);return false;"><a class="ui-state-default" href="#">17</a></td>
                    <td class=" " onclick="DP_jQuery_1321382478403.datepicker._selectDay('#startDate',10,2011, this);return false;"><a class="ui-state-default" href="#">18</a></td>
                    <td class=" ui-datepicker-week-end " onclick="DP_jQuery_1321382478403.datepicker._selectDay('#startDate',10,2011, this);return false;"><a class="ui-state-default" href="#">19</a></td>
                </tr>
                <tr>
                    <td class=" ui-datepicker-week-end " onclick="DP_jQuery_1321382478403.datepicker._selectDay('#startDate',10,2011, this);return false;"><a class="ui-state-default" href="#">20</a></td>
                    <td class=" " onclick="DP_jQuery_1321382478403.datepicker._selectDay('#startDate',10,2011, this);return false;"><a class="ui-state-default" href="#">21</a></td>
                    <td class=" " onclick="DP_jQuery_1321382478403.datepicker._selectDay('#startDate',10,2011, this);return false;"><a class="ui-state-default" href="#">22</a></td>
                    <td class=" " onclick="DP_jQuery_1321382478403.datepicker._selectDay('#startDate',10,2011, this);return false;"><a class="ui-state-default" href="#">23</a></td>
                    <td class=" " onclick="DP_jQuery_1321382478403.datepicker._selectDay('#startDate',10,2011, this);return false;"><a class="ui-state-default" href="#">24</a></td>
                    <td class=" " onclick="DP_jQuery_1321382478403.datepicker._selectDay('#startDate',10,2011, this);return false;"><a class="ui-state-default" href="#">25</a></td>
                    <td class=" ui-datepicker-week-end " onclick="DP_jQuery_1321382478403.datepicker._selectDay('#startDate',10,2011, this);return false;"><a class="ui-state-default" href="#">26</a></td>
                </tr>
                <tr>
                    <td class=" ui-datepicker-week-end " onclick="DP_jQuery_1321382478403.datepicker._selectDay('#startDate',10,2011, this);return false;"><a class="ui-state-default" href="#">27</a></td>
                    <td class=" " onclick="DP_jQuery_1321382478403.datepicker._selectDay('#startDate',10,2011, this);return false;"><a class="ui-state-default" href="#">28</a></td>
                    <td class=" " onclick="DP_jQuery_1321382478403.datepicker._selectDay('#startDate',10,2011, this);return false;"><a class="ui-state-default" href="#">29</a></td>
                    <td class=" " onclick="DP_jQuery_1321382478403.datepicker._selectDay('#startDate',10,2011, this);return false;"><a class="ui-state-default" href="#">30</a></td>
                    <td class=" ui-datepicker-other-month ui-datepicker-unselectable ui-state-disabled">&nbsp;</td>
                    <td class=" ui-datepicker-other-month ui-datepicker-unselectable ui-state-disabled">&nbsp;</td>
                    <td class=" ui-datepicker-week-end ui-datepicker-other-month ui-datepicker-unselectable ui-state-disabled">&nbsp;</td>
                </tr>
            </tbody>
        </table>
    </div>

	<div class="UI-mainBg Pagination" id="paginatedGridPagination">
        <div class="PaginationContainer">
            <div class="Bar">
                <a class="previousPage" href="javascript:void(0);"><span>&lt;&lt;Previous</span></a>
                <span class="pages"></span>
                <a class="nextPage" href="javascript:void(0);"><span>Next &gt;&gt;</span></a>
                <span class="ClearAll clr"></span>
            </div>
            <div style="" class="PageSize">
                Results Per Page:
                <select class="pageSize">
                    <option value="15">15</option>
                    <option value="20">20</option>
                    <option value="50">50</option>
                    <option value="100">100</option>
                </select>
            </div>
            <span class="ClearAll clr"></span>
        </div>
    </div>
</asp:Content>