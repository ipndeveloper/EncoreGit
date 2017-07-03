
        /***********************************************************************
        * Varibales Globales
        ***********************************************************************/
        var vg_sTodosCaracteres = ' !"#$%&\'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyzáéíóú{|}~';
        var vg_sSoloLetras = 'abcdefghijklmnñopqrstuvwxyzáéíóú';
        var vg_sSoloNumeros = '-1234567890';
        var vg_sSoloNumerosDecimales = '-1234567890';

        $(document).ready(function () {

            $("input[name*='solo_letras']").fn_util_validarLetras();
            $("input[name*='solo_numeros']").fn_util_validarNumeros();

        });




        // **************************************************************************
        //   Funciones Desavilitar y habilitar Controles
        // **************************************************************************
        function fn_util_seteaObjetoDeshabilitado(sIdInput) {
            $(sIdInput).addClass('css_input_deshabilitado');
            $(sIdInput).attr("disabled", true)
        }

        function fn_util_seteaObjetoHabilitado(sIdInput) {
            $(sIdInput).addClass('css_campos').removeClass('css_input_deshabilitado');
            $(sIdInput).attr("disabled", false)
        }
        function fn_util_seteaBotonDeshabilitado(sIdInput) {
            $(sIdInput).addClass('css_boton_deshabilitado');
            $(sIdInput).attr("disabled", true)
        }

        function fn_util_seteaBotonHabilitado(sIdInput) {
            $(sIdInput).addClass('css_boton').removeClass('css_boton_deshabilitado');
            $(sIdInput).attr("disabled", false)
        }


        // **************************************************************************
        //   Funciones Validaciones Númericas y Alfanumericas
        // **************************************************************************
        (function (a) {
            a.fn.fn_util_validarLetras = function () {
                var id = jQuery(this).attr("id");
                a(this).on({
                    keypress: function (a) {

                        var c = a.which, //c dfgdfgdfgdfg
                    d = a.keyCode,
                    e = String.fromCharCode(c).toLowerCase(),
                    f = vg_sSoloLetras;
                        (-1 != f.indexOf(e) || 9 == d || 37 != c && 37 == d || 39 == d && 39 != c || 8 == d || 46 == d && 46 != c) && 161 != c || a.preventDefault()
                    },
                    focusout: function (a) {
                        var pDiferente = new RegExp("[" + vg_sSoloLetras + "]", 'gi');
                        var sDiferente = vg_sTodosCaracteres.replace(pDiferente, '');
                        var pFinal = new RegExp("[" + sDiferente + "]", 'gi');
                        var sFinal = jQuery(this).val();
                        sFinal = sFinal.replace(pFinal, '');
                        jQuery(this).val(sFinal);
                    }
                })
            }
        })(jQuery);

        (function (a) {
            a.fn.fn_util_validarNumeros = function () {
                var id = jQuery(this).attr("id");
                a(this).on({
                    blur: function () {
                        var valor = $(this).val();

                        if (valor == '') {
                            $(this).val('0');
                        }

                        var pDiferente = new RegExp("[" + vg_sSoloNumeros + "]", 'gi');
                        var sDiferente = vg_sTodosCaracteres.replace(pDiferente, '');
                        var pFinal = new RegExp("[" + sDiferente + "]", 'gi');
                        var sFinal = jQuery(this).val();
                        sFinal = sFinal.replace(pFinal, '');
                        jQuery(this).val(sFinal);

                    },
                    keypress: function (a) {
                        var c = a.which, //c dfgdfgdfgdfg
                    d = a.keyCode,
                    e = String.fromCharCode(c).toLowerCase(),
                    f = vg_sSoloNumeros;
                        (-1 != f.indexOf(e) || 9 == d || 37 != c && 37 == d || 39 == d && 39 != c || 8 == d || 46 == d && 46 != c) && 161 != c || a.preventDefault()
                    }
                })
            }
        })(jQuery);
        /***********************************************************************
        * jQuery fn_util_validarCampoDecimal function
        * Version 1
        * Validar Decimales: cantidad decimal, signo de puntuacion (',' - '.' ), cantidad de enteros
        * Copyright (c) 2015
        **********************************************************************/
        (function (a) {

       
            a.fn.fn_util_validaDecimal = function (longitud) {
                var id = jQuery(this).attr("id");
                var caracter = ".";
                jQuery(this).addClass("css_textoDecimal");
                a(this).on({
                    blur: function () {
                        var valor = $(this).val();
                        $(this).val(fn_util_ValidaMonto(valor, longitud));
                    },
                    keypress: function (event) {
                        var valor = $(this).val();

                        var c = event.which,
                    d = event.keyCode,
                    e = String.fromCharCode(c).toLowerCase(),
                    f = vg_sSoloNumerosDecimales + caracter;

                        (-1 != f.indexOf(e) || 9 == d || 37 != c && 37 == d || 39 == d && 39 != c || 8 == d || 46 == d && 46 != c) && 161 != c || event.preventDefault();

                        var key = String.fromCharCode(event.which);
                        var position = $(this).fn_util_obtenerPosicionCursor() - 1;

                        //alert(position + "|" + (valor.indexOf(caracter)));
                        if (position < (valor.indexOf(caracter))) {

                        } else if ((valor.indexOf(caracter) != -1) && (valor.substring(valor.indexOf(caracter), valor.indexOf(caracter).length).length > longitud)) {
                            event.preventDefault();
                        }

                        //Validar el punto / coma
                        if ((valor.indexOf(caracter) != -1) && e == caracter) {
                            event.preventDefault();
                        }

                        //Validar el negativo
                        if ((valor.indexOf('-') != -1) && e == '-') {
                            event.preventDefault();
                        }
                    }
                });
            }
        })(jQuery);

        (function (a) {

            a.fn.fn_util_validarCampoDecimal = function (b, caracter, longitud) {
                var id = jQuery(this).attr("id");

                a(this).on({
                    blur: function () {
                        var valor = $(this).val();
                        var ceros = '';
                        var rango = ((valor.length) - (valor.indexOf(caracter))) - 1;
                        var cantidad = '';
                        var totalLong = 0;
                        var totalValor = 0;

                        if (valor == '') {
                            totalLong = longitud;
                            totalValor = "0" + caracter;

                        } else if (valor.indexOf(caracter) != -1 && rango > longitud) {
                            var decimal = parseFloat(valor);
                            totalValor = decimal.toFixed(longitud);

                        } else if (valor.indexOf(caracter) == 0) {
                            var corte = valor.split(caracter);
                            cantidad = corte[1].length;
                            totalLong = longitud - cantidad;
                            totalValor = "0" + valor;

                        } else if (valor.indexOf(caracter) != -1) {
                            var corte = valor.split(caracter);
                            cantidad = corte[1].length;
                            totalLong = longitud - cantidad;
                            totalValor = valor;

                        } else {
                            totalLong = longitud;
                            totalValor = valor + caracter;
                        }

                        for (var i = 1; i <= totalLong; i++) {
                            ceros = ceros + "0";
                        }

                        $(this).val(totalValor + ceros);

                        var str = valor;
                        var n = str.split('');
                        var valores = b + caracter;
                        for (var i = 0; i <= n.length - 1; i++) {
                            var index = valores.indexOf(n[i]);

                            if (index == -1) {
                                $(this).val('');
                                return;
                            }
                        }
                    },
                    keypress: function (event) {
                        var valor = $(this).val();

                        var c = event.which,
                    d = event.keyCode,
                    e = String.fromCharCode(c).toLowerCase(),
                    f = b + caracter;

                        (-1 != f.indexOf(e) || 9 == d || 37 != c && 37 == d || 39 == d && 39 != c || 8 == d || 46 == d && 46 != c) && 161 != c || event.preventDefault();

                        var key = String.fromCharCode(event.which);
                        var position = $(this).fn_util_obtenerPosicionCursor() - 1;

                        if (position < (valor.indexOf(caracter))) {

                        } else if ((valor.indexOf(caracter) != -1) && (valor.substring(valor.indexOf(caracter), valor.indexOf(caracter).length).length > longitud)) {
                            event.returnValue = false;
                        }

                        //Validar el punto / coma
                        if ((valor.indexOf(caracter) != -1) && e == caracter) {
                            event.preventDefault();
                        }

                        //Validar el negativo
                        if ((valor.indexOf('-') != -1) && e == '-') {
                            event.preventDefault();
                        }
                    }
                });
            }
        })(jQuery);


        //**********************************************************************
        // Nombre: fn_util_ValidaMonto
        //**********************************************************************
        function fn_util_ValidaMonto(pstrMonto, pintDecimales) {


            
            var strMonto = String(pstrMonto);
            if (fn_util_trim(strMonto) == "") strMonto = "0";

            $('<input>').attr({
                type: 'hidden',
                id: 'hddUtilMonto',
                name: 'hddUtilMonto'
            }).appendTo('body');

            $("#hddUtilMonto").val($.number(pstrMonto, 2));
            return $("#hddUtilMonto").val();
        }

        
        /***********************************************************************
        * funciones Utilitarias
        * Version 2
        * obtener Posicion de Cursor, Obtener Trama JSON, Activar Linea de Tabla
        * Copyright (c) 2014
        **********************************************************************/
        (function (a) {
            $.fn.fn_util_obtenerPosicionCursor = function () {
                var el = $(this).get(0);
                var pos = 0;
                if ('selectionStart' in el) {
                    pos = el.selectionStart;
                } else if ('selection' in document) {
                    el.focus();
                    var Sel = document.selection.createRange();
                    var SelLength = document.selection.createRange().text.length;
                    Sel.moveStart('character', -el.value.length);
                    pos = Sel.text.length - SelLength;
                }
                return pos;
            }
        })(jQuery);


        //**********************************************************************
        // Nombre: fn_util_trim
        //**********************************************************************
        function fn_util_trim(cadena) {
            nuevaCadena = cadena.toString();

            if (nuevaCadena.length > 0) {
                nuevaCadena = $.trim(nuevaCadena);
            } else {
                nuevaCadena = '';
            }
            return nuevaCadena;
        };


        //**********************************************************************
        // Nombre:  Modo Carga Combos Ajax
        //**********************************************************************
        function fn_agregaCargandoAjax(pIdObjeto) {
            var sIdImage = pIdObjeto.replace('#', '');
            $(pIdObjeto).parent().append('<img id="img_ajax_cargando_' + sIdImage + '" src="<%= ResolveUrl("~/Content/Images/Icons/ajax-loader.gif") %>" />');
        }

        function fn_removerCargandoAjax(pIdObjeto) {
            var sIdImage = pIdObjeto.replace('#', '');
            $('#img_ajax_cargando_' + sIdImage).remove();
        }