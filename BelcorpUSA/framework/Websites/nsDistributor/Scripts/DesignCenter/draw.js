
MyCurrentCanvasCallBacks = {};
MyCurrentCanvasCallBacks.isProcessing = false;
MyCurrentCanvasCallBacks.MainSize = { height: 500, width: 500 };

var canvasDrawApi = (function () {
    var registry = {},
        canvasInfoHandler = function (evt) {

            var novellaCanvas = getSelectedCanvas();
            var handlers = registry[evt.type];

            $.each(handlers, function (index, handler) {
                handler(evt, novellaCanvas);
            });
        },
        api;

    api = {
        bindEventsTo: function (element) {
            var eventName;
            for (eventName in registry) {
                if (registry.hasOwnProperty(eventName)) {
                    $(element).bind(eventName, canvasInfoHandler);
                }
            }
        },
        unBindEventsFrom: function (element) {
            var eventName;
            for (eventName in registry) {
                if (registry.hasOwnProperty(eventName)) {
                    $(element).unbind(eventName);
                }
            }
        },
        subscribe: function (eventName, eventHandler) {
            if (registry.hasOwnProperty(eventName)) {
                registry[eventName].push(eventHandler);
            } else {
                registry[eventName] = [eventHandler];
            }
        }
    };
    return api;
} ());

canvasDrawApi.subscribe('mousedown', function (evt, data) {
    data.position.x = data.position.prevX = evt.clientX - $('#DesignCanvas').offset().left;
    data.position.y = data.position.prevY = evt.clientY - $('#DesignCanvas').offset().top;
    replaceCanvasData(data.nv);
});

canvasDrawApi.subscribe('mouseup', function (evt, data) {
    data.position.x = data.position.y = data.position.prevX = data.position.prevY = null;
    replaceCanvasData(data.nv);
});

canvasDrawApi.subscribe('mouseleave', function (evt, data) {
    data.position.x = data.position.y = data.position.prevX = data.position.prevY = null;
    replaceCanvasData(data.nv);
});

canvasDrawApi.subscribe('mousemove', function (evt, data) {
    if (data.position.x == null || data.position.y == null) {
        return;
    }
    data.position.x = evt.clientX - $('#DesignCanvas').offset().left;
    data.position.y = evt.clientY - $('#DesignCanvas').offset().top;
    if (data.position.prevX == null) {
        data.position.prevX = x;
    }
    if (data.position.prevY == null) {
        data.position.prevY = y;
    }

    //Figure out whether to add or subtract x and y based on the current rotation
    var factors = {
        "-315": {
            x: (((data.position.y - data.position.prevY) + (data.position.x - data.position.prevX)) / Math.sqrt(2)),
            y: (((data.position.prevX - data.position.x) - (data.position.prevY - data.position.y)) / Math.sqrt(2))
        },
        "-270": {
            x: data.position.y - data.position.prevY,
            y: data.position.prevX - data.position.x
        },
        "-225": {
            x: (((data.position.y - data.position.prevY) - (data.position.x - data.position.prevX)) / Math.sqrt(2)),
            y: (((data.position.prevX - data.position.x) + (data.position.prevY - data.position.y)) / Math.sqrt(2))
        },
        "-180": {
            x: data.position.prevX - data.position.x,
            y: data.position.prevY - data.position.y
        },
        "-135": {
            x: (((data.position.prevY - data.position.y) + (data.position.prevX - data.position.x)) / Math.sqrt(2)),
            y: (((data.position.x - data.position.prevX) - (data.position.y - data.position.prevY)) / Math.sqrt(2))
        },
        "-90": {
            x: data.position.prevY - data.position.y,
            y: data.position.x - data.position.prevX
        },
        "-45": {
            x: (((data.position.prevY - data.position.y) - (data.position.prevX - data.position.x)) / Math.sqrt(2)),
            y: (((data.position.x - data.position.prevX) + (data.position.y - data.position.prevY)) / Math.sqrt(2))
        },
        "0": {
            x: data.position.x - data.position.prevX,
            y: data.position.y - data.position.prevY
        },
        "45": {
            x: (((data.position.y - data.position.prevY) + (data.position.x - data.position.prevX)) / Math.sqrt(2)),
            y: (((data.position.prevX - data.position.x) - (data.position.prevY - data.position.y)) / Math.sqrt(2))
        },
        "90": {
            x: data.position.y - data.position.prevY,
            y: data.position.prevX - data.position.x
        },
        "135": {
            x: (((data.position.y - data.position.prevY) - (data.position.x - data.position.prevX)) / Math.sqrt(2)),
            y: (((data.position.prevX - data.position.x) + (data.position.prevY - data.position.y)) / Math.sqrt(2))
        },
        "180": {
            x: data.position.prevX - data.position.x,
            y: data.position.prevY - data.position.y
        },
        "225": {
            x: (((data.position.prevY - data.position.y) + (data.position.prevX - data.position.x)) / Math.sqrt(2)),
            y: (((data.position.x - data.position.prevX) - (data.position.y - data.position.prevY)) / Math.sqrt(2))
        },
        "270": {
            x: data.position.prevY - data.position.y,
            y: data.position.x - data.position.prevX
        },
        "315": {
            x: (((data.position.prevY - data.position.y) - (data.position.prevX - data.position.x)) / Math.sqrt(2)),
            y: (((data.position.x - data.position.prevX) + (data.position.y - data.position.prevY)) / Math.sqrt(2))
        }
    },
      factor = factors[data.degree],
      xFactor = factor.x,
      yFactor = factor.y;

    //Add the scale factor
    xFactor = xFactor / data.scale;
    yFactor = yFactor / data.scale;

    data.position.centerX = data.position.centerX + xFactor;
    data.position.centerY = data.position.centerY + yFactor;
    data.position.prevX = data.position.x;
    data.position.prevY = data.position.y;
    data.reDrawImage();
    replaceCanvasData(data);

});


function mainButtonsUI(hasImage) {
    var getClassName;
    if (hasImage) {
        $('#MainBtns').removeClass('noImage');
        getClassName = activate;
    }
    else {
        $('#MainBtns').addClass('noImage');
        getClassName = deactivate;
    }

    $('#MainBtns .UI-icon').each(function () {
        var btnStatus = $(this).attr('class');
        btnStatus = getClassName(btnStatus);
        if (!$(this).hasClass(btnStatus)) {
            $(this).attr('class', btnStatus);
        }
    });

    function activate(currentClass) {
        return currentClass.replace('-deactive', '');
    }

    function deactivate(currentClass) {
        if (currentClass.indexOf('-deactive') > 0) {
            return currentClass;
        }
        else {
            return currentClass + '-deactive';
        }
    }
}


function replacePreviewNovellaAfterApproval() {
    var $selectedImages = $('#novellaDesignItems_UL .selected img'),
    myCanvas = getSelectedCanvas();

    if ($selectedImages.length == 4 || $selectedImages.length == 2) {
        $selectedImages.last().remove();
    }
    else if ($selectedImages.length == 3) {
        $selectedImages.filter('img:nth-child(2)').hide();
    }

    if (myCanvas.isApproved) {
        var height = $selectedImages.height(),
            width = $selectedImages.width(),
            orderItemId = $selectedImages.parents("li").attr("id");

        SaveCanvasImage('PreviewCanvas' + myCanvas.index, 'PreviewImageFilePath', orderItemId, null, null);

        var oImgElement = document.createElement("img");
        oImgElement.height = height;
        oImgElement.width = width;
        oImgElement.src = document.getElementById('PreviewCanvas' + myCanvas.index).toDataURL("image/png");
        $('#novellaDesignItems_UL .selected').append(oImgElement);
        $selectedImages.first().hide();
    }
    else {
        $selectedImages.first().show();
    }
}

function onDesignInit() {
    initializeMainAndPreviewCanvas();
    addNovellaCanvas();
    //Save Progress
    $('#btnSaveProgress').click(function () {
        if (!$('#btnSaveProgress').hasClass('ButtonInactive')) {
            var myCanvas = getSelectedCanvas();
            if (typeof myCanvas !== "undefined") {
                var index = myCanvas.index,
                selectedLi = $('#novellaDesignItems_UL .selected'),
                frame = selectedLi.data('frame'),
                orderItemId = selectedLi.attr("id"),
                previousScale = myCanvas.scale,
                mainId = 'MainCanvas' + index,
                previewId = 'PreviewCanvas' + index,
                thumbnail = selectedLi.children("img").first(),
                height = thumbnail.height(),
                width = thumbnail.width(),
                imgInfo = getImgInfo(myCanvas.src);

                if ($('#' + mainId).length < 1) {
                    thumbnail.hide();
                    removeAndRename(previewId, 'PreviewCanvas', 'PreviewWrapper');
                    removeAndRename(mainId, 'MainCanvas', 'SelectedImage');
                    $('#MainCanvas').hide();
                } else {
                    selectedLi.children("img:visible").remove();
                }

                $('#PleaseWaitWhileWeUploadYourFiles').show();
                $('#savingNovellasInProgress').jqmShow();

                MyCurrentCanvasCallBacks.rc = new MyRequestsCompleted({
                    numRequest: 1,
                    singleCallBack: function () {
                        $('#savingNovellasInProgress div').children().hide();
                        $('#savingNovellasInProgress').jqmHide();
                    }
                });

                var newCanvas = createNewCanvasToSave('tempPreviewCanvas', frame.width, frame.height);
                if (!!(newCanvas.getContext && newCanvas.getContext('2d'))) {
                    var newCanvasContext = newCanvas.getContext('2d');
                    var main = document.getElementById(mainId);
                    var data = main.getContext('2d').getImageData(0, 0, main.width, main.height);
                    newCanvasContext.putImageData(data, -((main.width / 2) - (frame.width / 2)), -((main.height / 2) - (frame.height / 2)));
                    $(newCanvas).appendTo(selectedLi);
                    newCanvasContext.drawImage(document.getElementById('PreviewFrame'), 0, 0);
                    SaveCanvasImage("tempPreviewCanvas", 'PreviewImageFilePath', orderItemId, null, null);
                }


                var tempInfo = copyNovellaCanvas(myCanvas);
                var mainCanvasSize = getMainCanvasSize(imgInfo, myCanvas);
                var mainCanvas;

                //any kind of rotation save as is
                if (mainCanvasSize.dw === "") {
                    mainCanvas = createNewCanvasToSave('', imgInfo.canvasWidth, imgInfo.canvasHeight);
                } else {
                    //upright or side crop unecessary edges
                    mainCanvas = createNewCanvasToSave('', mainCanvasSize.dw, mainCanvasSize.dh);
                }

                var mainCanvasAssetContext = mainCanvas.getContext('2d');
                var render = new canvasRender(null, tempInfo, null, {
                    context: mainCanvasAssetContext,
                    halfWidth: mainCanvas.width / 2,
                    halfHeight: mainCanvas.height / 2,
                    width: mainCanvas.width,
                    height: mainCanvas.height
                });
                render.drawAsset(true);

                SaveCanvasImage('', "MainCanvasFilePath", orderItemId, mainCanvas);

                var img = $("#tempPreviewCanvas")[0].toDataURL("image/png");
                $("#tempPreviewCanvas").remove();
                selectedLi.append('<img height="' + height + '" width="' + width + '" src="' + img + '"/>');
            }
        }
    });

    //Reset  Image
    $('#ResetPic').click(function () {
        var myCanvas = getSelectedCanvas();
        if (canvasIsProcessing() || myCanvas == null || typeof myCanvas === "undefined") {
            return false;
        }
        if (myCanvas.src != undefined && myCanvas.src != null) {
            var bottomPercentage = checkBrowser() ? '50%' : '0%';
            putInEditMode(myCanvas);

            inEditMode();

            $('#Lighten .ui-slider-handle').css('bottom', bottomPercentage);
            $('a.btnToolBar').removeClass('btnSelected');

            if (!$('a.move').hasClass('locked')) {
                $('a.move').addClass('btnSelected');
            }
            $('#PreviewCanvas, #MainCanvas').css({ filter: 'none' });

            myCanvas.reset();
            myCanvas.addImageDraw(myCanvas.srcPath);
            updateCanvas(myCanvas);
            $('#Size .ui-slider-handle').css('bottom', (myCanvas.scale * 100 + '%'));
        }
        return false;
    });

    $('#DesignComplete').click(function () {
        if (canvasIsProcessing()) {
            return false;
        }
        if ($('#DesignComplete').hasClass('ButtonInactive') == false) {
            if (allApproved || allNovellasHasPreviews()) {
                $('#PleaseWaitWhileWePrepareYourItemsForReview').show();
                $('#savingNovellasInProgress').jqmShow();

                var orderItemId = $('#OrderItemID').val();
                var bundleGuid = $('#BundleID').val();
                var productId = $('#ProductID').val();

                $.post('/Design/PreviewWindow', { Id: productId, bundleGuid: bundleGuid, orderItemId: orderItemId }, function (data) {
                    $('#PreviewDesign').html(data);
                    $('#PreviewDesign').jqmShow();
                    $('#savingNovellasInProgress div').children().hide();
                    $('#savingNovellasInProgress').jqmHide();
                }, 'html');
            }
        }
        return false;
    });

    $('#Move').click(function () {
        var myCanvas = getSelectedCanvas();
        if (canvasIsProcessing() || myCanvas == null || typeof myCanvas === "undefined") {
            return false;
        }
        if (myCanvas.src != null) {
            putInEditMode(myCanvas);
        }
        return false;
    });

    //Lighten Image
    $('#Lighten').wrap('<div id="LightenSlider" class="hide">');
    $('div.lightenControl').hover(
                    function () {
                        if (!$('#MainBtns').hasClass('noImage')) {
                            $('#LightenSlider').show();
                        }
                    }, function () {
                        $('#LightenSlider').hide();
                    });

    var val = 50;
    var m = 0;
    if (!checkBrowser()) {
        val = 0;
        m = 50;
    }

    $("#Lighten").slider({
        orientation: 'vertical',
        range: 'min',
        min: m,
        max: 100,
        slide: function (event, ui) {
            var newLightenVal = ((ui.value / 100 * 2) - 1).toFixed(2);
            $("#lightenVal").val(newLightenVal);

            if (newLightenVal == 0) {
                $('#Light').removeClass('btnSelected');
            } else {
                $('#Light').addClass('btnSelected');
            }
            var novella = getSelectedCanvas();
            if (novella != null || typeof novella !== "undefined" || novella.src == null) {
                if (novella.src != undefined && novella.src != null) {
                    putInEditMode(novella);
                    novella.lightenVal = newLightenVal;
                    novella.reDrawImage();
                }
            }
        },
        value: val,
        step: 5
    });
    /***** End Lighten *****/

    /***** Start Size Image *****/
    $('#Size').wrap('<div id="SizeSlider" class="hide">');
    $('div.sizeControl').hover(
                    function () {
                        if (!$('#MainBtns').hasClass('noImage')) {
                            $('#SizeSlider').show();
                        }
                    }, function () {
                        $('#SizeSlider').hide();
                    });

    $("#Size").slider({
        orientation: 'vertical',
        range: 'min',
        min: 0,
        max: 100,
        slide: function (event, ui) {
            resizeCanvas(ui.value);
        },
        value: 50,
        step: 1
    });
    /***** End Size Image *****/

    /***** Start Rotate Image *****/
    $('a.rotator').click(function () {
        var novella = getSelectedCanvas();
        if (canvasIsProcessing() || typeof novella === "undefined" || novella == null || novella.src == null) {
            return false;
        }
        canvasBeginProcessing();
        if (novella.src != undefined && novella.src != null) {
            putInEditMode(novella);
            novella.rotateImage($(this).attr('rel'));
        }
    });
    /***** End Rotate Image *****/

    /***** Start Flip Image *****/
    $('a.flip').click(function () {
        var currentCanvas = getSelectedCanvas();
        if (canvasIsProcessing() || typeof currentCanvas === "undefined" || currentCanvas == null) {
            return false;
        }

        canvasBeginProcessing();
        var $this = $(this),
            selectedClass = 'btnSelected',
            isHorizontal = $this.hasClass('horizontal'),
            isVertical = $this.hasClass('vertical');
        if (currentCanvas.src != undefined && currentCanvas.src != null) {
            if ($this.hasClass(selectedClass)) {
                $this.removeClass(selectedClass);
            } else {
                $this.addClass(selectedClass);
            }
            putInEditMode(currentCanvas);
            currentCanvas.needsToFlipY = isVertical;
            currentCanvas.needsToFlipX = isHorizontal;
            if (isHorizontal) {
                currentCanvas.isFlipX = !currentCanvas.isFlipX;
            }
            if (isVertical) {
                currentCanvas.isFlipY = !currentCanvas.isFlipY;
            }
            currentCanvas.reDrawImage();
        }
    });
    /***** End Flip Image *****/
    $('#Desaturate').click(function () {
        var currentCanvas = getSelectedCanvas();
        if (canvasIsProcessing() || typeof currentCanvas === "undefined" || currentCanvas == null || novella.src == null) {
            return false;
        }
        canvasBeginProcessing();
        if (currentCanvas.src != undefined && currentCanvas.src != null) {
            putInEditMode(currentCanvas);
            currentCanvas.isDesat = !currentCanvas.isDesat;
            currentCanvas.reDrawImage();
        }
        return false;
    });

    //Clicking an image
    $("body").delegate('#Library img,#mediaLibrary img', "click", function () {

        var btnStatus = $('#Move .UI-icon').attr('class'),
            theSrcImg = $(this).parent().find('input:hidden').val();
        //           theSrcImg = $(this).attr('src');
        if (btnStatus.contains('-locked')) {
            btnStatus = btnStatus.replace('-locked', '');
            $('#Move .UI-icon').attr('class', btnStatus);
            $('#Move').addClass('btnSelected');
        }
        $('div.selectImageText').hide();
        $('#PreviewFrame, #SelectedImage').show();
        $('#mediaLibrary').jqmHide();
        $('#DesignApproval').removeClass('ButtonInactive');
        $('div.previewItemActions input').removeAttr('disabled');
        $('ul.designItems').addClass('inactive');
        mainButtonsUI(true);

        //Decide to make a new one or to load a prior one.        
        var myCanvas = getSelectedCanvas();
        if (typeof myCanvas === "undefined") {
            return false;
        }
        if (myCanvas.isApproved) {
            $('#loseAllChangesRestart').data('imgSrc', theSrcImg);
            $('#ChangesLost').jqmShow();
        }
        else {
            approvalBind();
            myCanvas.addImageDraw(theSrcImg);
            updateCanvas(myCanvas);
        }
        return false;
    });

    $('#loseAllChangesRestart').click(function () {
        var mycanvas = getSelectedCanvas();
        if (typeof mycanvas === "undefined") {
            return false;
        }
        var index = mycanvas.index;
        $('ul.designItems li.selected').data('novellaCanvas', new novellaCanvas(false, false, false, 0, 1, index));
        $('#PreviewCanvas, #MainCanvas, #PreviewFrame').show();
        $('#MainCanvas' + index).remove();
        $('#PreviewCanvas' + index).remove();
        removeImage();
        $('#DesignApproval').removeClass('ButtonInactive');
        $('ul.designItems li.selected').data('novellaCanvas').addImageDraw($('#loseAllChangesRestart').data('imgSrc'));
        approvalBind();
        $('#DesignComplete').addClass('ButtonInactive');
        $('#loseAllChangesRestart').data('imgSrc', null);
        $('#ChangesLost').jqmHide();
        replacePreviewNovellaAfterApproval();
        $('.designPreviewItem').children('img[id^="previewAsset"]').remove();
        return false;
    });

    //**Clicking a Novella**//
    $('ul.designItems li').click(function () {
        var previous = getSelectedCanvas();
        if (previous != undefined && previous.isApproved) {
            previous.isEditing = false;
        }

        $('ul.designItems').children().removeClass('selected');
        var currentLi = $(this),
        currentCanvas = $(this).data('novellaCanvas');
        currentLi.addClass('selected');
        $('.designPreviewItem').children('img[id^="previewAsset"]').remove();
        previewRatioAndWrap(currentLi.children("img").first().attr("src"), currentLi, currentCanvas);
    });
    return false;
}

function clickingNovellaLi(currentLi, currentCanvas, frame) {
    if (typeof currentCanvas === "undefined") {
        return false;
    }
    currentCanvas.frame = frame;
    replaceCanvasData(currentCanvas);
    updateCanvas(currentLi, currentCanvas);
    showSelectedCanvas(currentLi);
    $('div.selectImageText').hide();
    if (currentCanvas.isApproved) {
        $('div.allDesignsApproved').hide();
        $('#preview, div.selectedImage').show();
        $('#PreviewFrame, #PreviewWrapper').show().css({ visibility: 'visible' });
        $('div.previewItemActions span').removeClass('ButtonInactive');
        $('div.previewItemActions input').removeAttr('disabled');
        $('#MainBtns').removeClass('noImage');
        $('#DesignApproval').addClass('ButtonInactive');
        $('#MainBtns .UI-icon').each(function () {
            var btnStatus = currentLi.attr('class');
            btnStatus = btnStatus.replace('-deactive', '');
            currentLi.attr('class', btnStatus);
        });
        $('.icon-move').removeClass('icon-move').addClass('icon-move-locked');
        $('ul.designItems').addClass('inactive');
        $('#DesignApproval').unbind('click');
        $('#Size .ui-slider-handle').css('bottom', currentCanvas.scale * 100 + '%');

    }
    else {
        if (currentCanvas.src == undefined || currentCanvas.src == null) {
            $('div.selectImageText').show();
            $('#DesignApproval').unbind('click');
            $('#DesignApproval').addClass('ButtonInactive');
            mainButtonsUI(false);
        }
        else {
            $('#DesignApproval').removeClass('ButtonInactive');
            approvalBind();
            mainButtonsUI(true);
        }
    }
    if (checkApprovals() || allNovellasHasPreviews()) {
        $('#DesignComplete').removeClass('ButtonInactive');
    }

    if (currentLi.find('input:hidden[name="IsEditing"]').val() === 'False') {
        $('#btnSaveProgress').addClass('ButtonInactive');
    } else {
        $('#btnSaveProgress').removeClass('ButtonInactive');
    }
    return false;
}

/***************************
Functions
***************************/
function inEditMode() {
    //Hide all saved canvases.
    $('#SelectedImage').find('canvas').hide();
    $('#PreviewWrapper').find('canvas').hide();

    //Show the editing canvases
    $('#PreviewCanvas, #PreviewFrame, #MainCanvas').show();
    removeImage();

    $('#Move').children('.UI-icon').removeClass('icon-move-locked').addClass('icon-move');
    $('#DesignApproval').removeClass('ButtonInactive');
}

function putInEditMode(theCanvas) {
    if (typeof theCanvas === "undefined") {
        return false;
    }
    if (theCanvas.isApproved && !theCanvas.isEditing) {
        removeImage();
        reDrawNovellaCanvas();
        approvalBind();
        $('#Move').children('.UI-icon').removeClass('icon-move-locked').addClass('icon-move');
        theCanvas.isEditing = true;
        $('#DesignApproval').removeClass('ButtonInactive');
        $('#DesignComplete').addClass('ButtonInactive');
        approvalBind();
        $('.designPreviewItem').children('img[id^="previewAsset"]').remove();
        mainButtonsUI(true);
    }
    return false;
}

function resizeCanvas(val) {
    var novella = getSelectedCanvas();
    if (typeof novella === "undefined") {
        return false;
    }
    if (novella.src != undefined && novella.src != null) {
        putInEditMode(novella);
        novella.scale = (val / 100).toFixed(2);
        novella.reDrawImage();
    }
    return false;
}

function approvalBind() {
    $('#DesignApproval').unbind("click").bind("click", doNovellaApproved);
}

function ensureCanvas(canvas) {
    if (typeof FlashCanvas != "undefined") {
        FlashCanvas.initElement(canvas);
    }
}

var doNovellaApproved = function () {
    var myCanvas = getSelectedCanvas();
    if (typeof myCanvas === "undefined") {
        return false;
    }
    $('#DesignApproval').addClass('ButtonInactive');

    $('#PleaseWaitWhileWeUploadYourFiles').show();
    $('#savingNovellasInProgress').jqmShow();

    MyCurrentCanvasCallBacks.rc = new MyRequestsCompleted({
        numRequest: 3,
        singleCallBack: function () {
            $('#savingNovellasInProgress div').children().hide();
            $('#savingNovellasInProgress').jqmHide();
            selectNextUnapprove();
        }
    });

    var selectedLi = $('#novellaDesignItems_UL li.selected'),
        index = myCanvas.index,
        offSet = getPreviewOffSet(),
        mainId = 'MainCanvas' + index,
        previewId = 'PreviewCanvas' + index,
        $preview = $('#' + previewId),
        frame = selectedLi.data('frame'),
        imgInfo = getImgInfo(myCanvas.src);

    selectedLi.find('input:hidden[name="MainPreviewPath"]').remove();
    selectedLi.children('img[id^="previewAsset"]').remove();
    selectedLi.find('input:hidden[name="IsEditing"]').val("False");

    var previewFrame = createNewCanvasToSave(previewId, frame.width, frame.height);
    if (!!(previewFrame.getContext && previewFrame.getContext('2d'))) {
        var contextPreviewFrame = previewFrame.getContext('2d');
                
        contextPreviewFrame.drawImage(document.getElementById('PreviewCanvas'), 0, 0);
        contextPreviewFrame.drawImage(document.getElementById('PreviewFrame'), 0, 0);
        $preview.remove();
        $(previewFrame).appendTo('#PreviewWrapper');

        SaveFulFillment(myCanvas, frame, imgInfo, selectedLi);

        if ($preview.is(':visible') != true) {
            removeAndRename(mainId, 'MainCanvas', 'SelectedImage');
        }
    }


    $('#PreviewFrame, #PreviewCanvas').hide();
    //unbind events from this canvas
    canvasDrawApi.unBindEventsFrom($("#" + mainId));

    if ($('#MainCanvas').length == 0) {
        createCanvasAppendTo('MainCanvas', 'SelectedImage');
    }
    myCanvas.isApproved = true;
    myCanvas.isEditing = false;
    $('.icon-move').removeClass('icon-move').addClass('icon-move-locked');
    $('#DesignApproval').unbind('click');
    replacePreviewNovellaAfterApproval();
    allApproved(checkApprovals());
    return false;
};


function canFit(boundingBox, sourceBox) {
    return boundingBox.width >= sourceBox.width && boundingBox.height >= sourceBox.height;
}

function determineScale(boundingBox, sourceBox) {

    if (canFit(boundingBox, sourceBox)) {
        return 1;
    }
    else {
        return determineScaleFactor(boundingBox, sourceBox);
    }
}

function determineScaleFactor(boundingBox, sourceBox) {
    var widthScale = boundingBox.width / sourceBox.width;
    var heightScale = boundingBox.height / sourceBox.height;
    return (widthScale < heightScale) ? widthScale : heightScale;
}


function addNovellaCanvas() {
    $('ul.designItems li').each(function (index) {
        $(this).data('novellaCanvas', new novellaCanvas(false, false, false, 0, 1, index));
    });
}

function getSelectedCanvas() {
    return $('ul.designItems li.selected').data('novellaCanvas');
}

function reDrawNovellaCanvas() {
    var select = getSelectedCanvas();
    if (typeof select === "undefined") {
        return false;
    }
    var index = select.index,
        scale = select.scale,
        main = $("#MainCanvas" + index),
        mainLeft = main.css("left"),
        mainTop = main.css("top"),
        mainSize = MyCurrentCanvasCallBacks.MainSize,
        preview = $("#PreviewCanvas" + index),
        frame = $('#novellaDesignItems_UL li.selected').data('frame');

    preview.hide();
    main.hide();

    $('#PreviewCanvas, #PreviewFrame, #MainCanvas').show();

    setHWforCanvasDraw($('#MainCanvas')[0], mainSize.width, mainSize.height);
    select.position.centerX = select.position.startCenterX = (mainSize.width / 2) / select.scale;
    select.position.centerY = select.position.startCenterY = (mainSize.height / 2) / select.scale;

    setHWforCanvasDraw($('#PreviewCanvas')[0], frame.width, frame.height);
    $('#MainCanvas').css({ left: mainLeft, top: mainTop, position: 'absolute' });
    select.needsToFlipX = select.isFlipX;
    select.needsToFlipY = select.isFlipY;
    select.scale = scale;
    select.reDrawImage();
    replaceCanvasData(select);
    return false;
}


function getPreviewOffSet(index) {
    var previewName = getCanvasName('#PreviewCanvas', index),
        mainName = getCanvasName('#MainCanvas', index),
        mainBasePosition = getBasePosition('#DesignCanvas', mainName),
        previewBasePosition = getBasePosition('#PreviewWrapper', previewName);

    var position = $('#DesignCanvas ' + mainName).position(),
                leftDifference = position.left - mainBasePosition.left,
                topDifference = position.top - mainBasePosition.top,
                newLeft = previewBasePosition.left + leftDifference,
                newTop = previewBasePosition.top + topDifference;

    return { left: newLeft, top: newTop };
}

function getCanvasName(name, index) {
    var value = typeof index === "undefined" ? "" : index;
    return name + value;
}

function centerCanvasMain(index) {
    var mainid = getCanvasName('#MainCanvas', index);
    var $mainCanvas = $(mainid),
        $canvasContainer = $('#DesignCanvas'),
        left = ($canvasContainer.width() - $mainCanvas.width()) / 2,
        top = ($canvasContainer.height() - $mainCanvas.height()) / 2;
    $mainCanvas.css({ left: left, top: top, position: 'absolute' });
}


function removeAndRename(canvasId, baseId, containerToAppendId) {
    if ($('#' + canvasId).length > 0) {
        $('#' + canvasId).remove();
    }

    $('#' + baseId).attr('id', canvasId);
    createCanvasAppendTo(baseId, containerToAppendId);
}

function createCanvasAppendTo(canvasToCreateId, containerToAppendId) {
    var shouldWireEvents = /^MainCanvas/.test(canvasToCreateId),
        canvas = document.createElement('canvas');
    ensureCanvas(canvas);
    document.getElementById(containerToAppendId).appendChild(canvas);
    $(canvas).attr('id', canvasToCreateId);
    if (shouldWireEvents) {
        canvasDrawApi.bindEventsTo(canvas);
    }
}


function showSelectedCanvas(current) {
    //Remove preview canvas/image if any
    //reset both preview and main canvas
    var canvasInfo = getSelectedCanvas();
    if (typeof canvasInfo === "undefined") {
        return false;
    }
    var index = canvasInfo.index,
    mainId = '#MainCanvas' + index,
        previewId = '#PreviewCanvas' + index;
    if (current.find('input:hidden[name="IsSharedVal"]').val() === "false") {
        $('#ShareThis').removeAttr('checked');
    } else {
        $('#ShareThis').attr('checked', 'checked');
    }

    inEditMode();

    if ($(mainId).length > 0) {
        $('#PreviewCanvas, #MainCanvas').hide();
        $(mainId + ',' + previewId).show();
        if (!canvasInfo.isEditing) {
            $('#PreviewFrame').hide();
        }
    }

    if (canvasInfo.showAsset === true && canvasInfo.isEditing === false) {
        $(previewId).hide();
    } else {
        $('.designPreviewItem').children('img[id^="previewAsset"]').remove();
    }
    canvasInfo.reDrawImage();
    return false;
}

function initializeMainAndPreviewCanvas() {
    createCanvasAppendTo('MainCanvas', 'SelectedImage');
    createCanvasAppendTo('PreviewCanvas', 'PreviewWrapper');

    var $designItemFirst = $('ul.designItems').children(':first');
    if ($designItemFirst.length > 0) {
        $('ul.designItems').children(':first').addClass('selected');
        if ($designItemFirst.children("img").length > 0) {
            previewRatioAndWrap($designItemFirst.children("img").attr("src"), $designItemFirst, getSelectedCanvas());
        }
    }
}

//Get frame ratio and fit the wrapping div to the correct size
function previewRatioAndWrap(theSrc, currentLi, currentCanvas) {
    var frame = currentLi.data('frame');
    $('div.designPreviewItem').css({ 'margin-left': frame.difference });
    $('#PreviewWrapper').width(frame.width).height(frame.height);
    $("#PreviewFrame").attr("src", theSrc);
    clickingNovellaLi(currentLi, currentCanvas, frame);
}



function checkBrowser() {
    if (!$.browser.msie || ($.browser.msie && parseInt($.browser.version) > 8)) {
        return true;
    } else {
        return false;
    }
}

function removeImage() {
    $('#PreviewCanvas, #MainCanvas').each(function () {
        var ctx = this.getContext('2d'),
            width = this.width,
            height = this.height;
        ctx.clearRect(0, 0, width, height);
    });
}

function allApproved(isApproved) {
    saveIsApprovedFlag(isApproved);
    if (isApproved) {
        $('div.selectImageText').hide();
        $('div.allDesignsApproved').show();
        $('#DesignComplete').removeClass('ButtonInactive');
        $('#novellaDesignItems_UL').children().removeClass('selected');
        $('#SelectedImage').find('canvas').hide();
        $('#PreviewWrapper').find('canvas').hide();
        $('input[name="DesignApproved"]').val('True');
    } else {
        $('input[name="DesignApproved"]').val('False');
    }
}

//Set current selected li novella to true or false.
function setSelectedNovella(val) {
    var current = $('#novellaDesignItems_UL').children('.selected').data('novellaCanvas');
    current.isApproved = val;
    $('#novellaDesignItems_UL').children('.selected').data('novellaCanvas', current);
}

//Checks all the novellas to see if they are approved or not.
function checkApprovals() {
    var allTrue = true;
    $('#novellaDesignItems_UL').children('li').each(function () {
        if ($(this).data('novellaCanvas').isApproved != true || $(this).data('novellaCanvas').isEditing) {
            $('div.allDesignsApproved').hide();
            allTrue = false;
            return false;
        }
    });
    return allTrue;
}


function getCanvasInfo(canvasId) {
    var main = $('#' + canvasId)[0];
    return {
        context: main.getContext('2d'),
        halfWidth: main.width / 2,
        halfHeight: main.height / 2,
        width: main.width,
        height: main.height
    };
}
/***************************
Functions
***************************/

function positionDetails() {
    this.prevX = null,
    this.prevY = null,
    this.x = null,
    this.y = null,
    this.centerX = null,
    this.centerY = null,
    this.startCenterX = null,
    this.startCenterY = null;
}

//************NOVELLA CANVAS OBJECT*************************//
function novellaCanvas(isFlipX, isFlipY, isApproved, degree, scale, index) {
    var defaultScale = scale;
    //Properties
    this.index = index;
    this.isFlipX = isFlipX;
    this.needsToFlipX = false;
    this.isFlipY = isFlipY;
    this.needsToFlipY = false;
    this.isApproved = isApproved;
    this.degree = degree;
    this.scale = scale;
    this.src = null;
    this.lightenVal = 0;
    this.isDesat = false;
    this.isEditing = true;
    this.position = new positionDetails();

    //Methods
    this.addImageDraw = addImageDraw;
    this.rotateImage = rotateImage;
    this.reDrawImage = reDrawImage;
    this.reset = function () {
        this.isFlipX = false;
        this.isFlipY = false;
        this.degree = 0;
        this.isDesat = false;
        this.scale = defaultScale;
        this.lightenVal = 0;
    };
}

function addImageDraw(srcPath, index) {

    this.reset();
    var self = this,
        myImg = new Image(),
        pc = getCanvasName('#PreviewCanvas', index),
        mc = getCanvasName('#MainCanvas', index),
        noIndex = typeof index === "undefined",
        draw = function () {
            $('#PleaseWaitWhileWeManipulateTheCanvas').hide();
            $('#savingNovellasInProgress').jqmHide();
            self.reDrawImage();
        };

    $('#PleaseWaitWhileWeManipulateTheCanvas').show();
    $('#savingNovellasInProgress').jqmShow();

    myImg.onload = function () {
        var previewC = $(pc)[0],
        mainC = $(mc)[0];
        var frame = $('#novellaDesignItems_UL li.selected').data('frame');

        setHWforCanvasDraw(previewC, frame.width, frame.height);
        self.position.centerX = self.position.startCenterX = (MyCurrentCanvasCallBacks.MainSize.width / 2) / self.scale;
        self.position.centerY = self.position.startCenterY = (MyCurrentCanvasCallBacks.MainSize.height / 2) / self.scale;
        setHWforCanvasDraw(mainC, MyCurrentCanvasCallBacks.MainSize.width, MyCurrentCanvasCallBacks.MainSize.height);

        self.scale = getImageScale(myImg);
        $('#Size .ui-slider-handle').css('bottom', (self.scale * 100) + '%');
        draw();

        if (noIndex) {
            centerCanvasMain();
        } else {
            centerCanvasMain(index);
            if (self.isApproved) {
                canvasDrawApi.unBindEventsFrom(mc);
            }
        }
    };

    this.src = myImg;
    this.srcPath = srcPath;
    myImg.src = srcPath;
}

function getImageScale(input) {
    var $bBox = $('#DesignCanvas'),
        bWidth = $bBox.css('width'),
        bHeight = $bBox.css('height');

    return determineScale({
        width: bWidth.substring(0, bWidth.length - 2),
        height: bHeight.substring(0, bHeight.length - 2)
    }, input);
}

function getImgInfo(theimg) {
    if (typeof theimg == "undefined") {
        return false;
    }

    var imgHeight = theimg.naturalHeight;
    var imgWidth = theimg.naturalWidth;

    if (typeof theimg.naturalHeight == "undefined") {
        imgHeight = theimg.height;
        imgWidth = theimg.width;
    }

    var constantRad = 45 * Math.PI / 180,
        canvasWidth = Math.abs((imgHeight) * Math.sin(constantRad)) + Math.abs((imgWidth) * Math.cos(constantRad)),
        canvasHeight = Math.abs((imgHeight) * Math.cos(constantRad)) + Math.abs((imgWidth) * Math.sin(constantRad));

    return {
        width: imgWidth,
        height: imgHeight,
        halfWidth: imgWidth / 2,
        halfHeight: imgHeight / 2,
        canvasWidth: canvasWidth,
        canvasHeight: canvasHeight,
        halfCanvasWidth: canvasWidth / 2,
        halfCanvasHeight: canvasHeight / 2
    };
}

function reDrawImage() {
    var renderIt = null;
    if ($("#MainCanvas").is(':visible')) {
        renderIt = new canvasRender("MainCanvas", this, "PreviewCanvas");
    } else {
        renderIt = new canvasRender("MainCanvas" + this.index, this, "PreviewCanvas" + this.index);
    }
    renderIt.reDraw();
}


function rotateImage(direction) {
    var isReverse = (this.isFlipY && !this.isFlipX) || (!this.isFlipY && this.isFlipX),
        isCounterClockwise = direction === 'ccw';

    this.isReverse = isReverse;
    this.isCounterClockwise = isCounterClockwise;

    if ((this.isCounterClockwise && this.isReverse) || (!this.isCounterClockwise && !this.isReverse)) {
        this.degree += 45;
    }
    else if ((this.isCounterClockwise && !this.isReverse) || (!this.isCounterClockwise && this.isReverse)) {
        this.degree -= 45;
    }

    if (this.degree >= 360 || this.degree <= -360) {
        this.degree = 0;
    }

    this.reDrawImage();
}

function setHWforCanvasDraw(canvas, newWidth, newHeight) {
    var ctx = canvas.getContext('2d');
    ctx.clearRect(0, 0, canvas.width, canvas.height);
    canvas.width = newWidth;
    canvas.height = newHeight;
}

function getBasePosition(containerSelector, canvasId) {
    var container = $(containerSelector),
            $canvas = container.find(canvasId),
            canvas = $canvas[0],
            baseLeft = (container.width() - canvas.width) / 2,
            baseTop = (container.height() - canvas.height) / 2;

    return { left: baseLeft, top: baseTop };
}

//************NOVELLA CANVAS OBJECT*************************//


//************CANVAS RENDER OBJECT*************************//
function canvasRender(canvasId, novellaCanvas, previewId, current) {

    this.previewId = previewId;
    if (canvasId == null) {
        this.current = current;
    } else {
        this.current = getCanvasInfo(canvasId);
    }
    this.novellaCanvas = novellaCanvas;
    this.needsToFlipX = novellaCanvas.needsToFlipX;
    this.needsToFlipY = novellaCanvas.needsToFlipY;
    this.imageManipulationHandlers = [this.brightness, this.desaturate];
}

canvasRender.prototype.reDraw = function () {
    this.flipCanvasHorizontal();
    this.flipCanvasVertical();
    this.rotateCanvas();
    this.manipulateImageData();
    this.mirrorPreview();
};

canvasRender.prototype.drawAsset = function (isMain) {
    this.flipCanvasHorizontal();
    this.flipCanvasVertical();
    if (isMain) {
        this.rotateMain();
    } else {
        this.rotateCanvas();
    }
    this.manipulateImageData();
};

canvasRender.prototype.mirrorPreview = function () {
    var data = this.current.context.getImageData(0, 0, this.current.width, this.current.height);
    var mirror = document.getElementById(this.previewId);
    var mirrorContext = mirror.getContext('2d');
    var frame = $('#novellaDesignItems_UL li.selected').data('frame');
    if (typeof frame == "undefined") {
        frame = this.frame;
    }
    mirrorContext.putImageData(data, -((this.current.width / 2) - (frame.width / 2)), -((this.current.height / 2) - (frame.height / 2)));
    canvasEndProcessing();
};


canvasRender.prototype.manipulateImageData = function () {
    var ctx = this.current.context,
        self = this,
        imageData = ctx.getImageData(0, 0, this.current.width, this.current.height);

    //let each handler do what it wants with the imagaData before putting it back
    $.each(this.imageManipulationHandlers, function (index, handler) {
        handler.apply(self, [imageData]);
    });
    ctx.putImageData(imageData, 0, 0, 0, 0, imageData.width, imageData.height);
};

canvasRender.prototype.brightness = function (imageData) {
    if (this.novellaCanvas.lightenVal != 0) {
        var amount = Math.max(-1, Math.min(1, this.novellaCanvas.lightenVal));
        var mul = amount + 1;
        var data = imageData.data;
        for (var i = 0; i < data.length; i += 4) {
            data[i] = mul * data[i]; // red
            data[i + 1] = mul * data[i + 1]; // green
            data[i + 2] = mul * data[i + 2]; // blue
        }
    }
};

canvasRender.prototype.desaturate = function (imageData) {
    if (this.novellaCanvas.isDesat) {
        var data = imageData.data;
        for (var i = 0; i < data.length; i += 4) {
            var brightness = 0.34 * data[i] + 0.5 * data[i + 1] + 0.16 * data[i + 2];
            data[i] = brightness; // red
            data[i + 1] = brightness; // green
            data[i + 2] = brightness; // blue
        }
    }
};

canvasRender.prototype.resetCanvas = function () {
    this.current.context.clearRect(0, 0, this.current.width, this.current.height);
};

canvasRender.prototype.rotateCanvas = function () {
    this.resetCanvas();

    var mc = this.current,
        nc = this.novellaCanvas,
        info = getImgInfo(nc.src);

    nc.dw = info.width * nc.scale;
    nc.dh = info.height * nc.scale;
    nc.dx = (mc.width - nc.dw) / 2;
    nc.dy = (mc.height - nc.dh) / 2;

    mc.context.save();
    mc.context.translate(mc.halfWidth, mc.halfHeight);
    mc.context.scale(nc.scale, nc.scale);
    mc.context.rotate(nc.degree * Math.PI / 180);
    mc.context.translate(-info.halfWidth + (nc.position.centerX - nc.position.startCenterX), -info.halfHeight + (nc.position.centerY - nc.position.startCenterY));

    mc.context.drawImage(nc.src, 0, 0);
    mc.context.restore();

};

canvasRender.prototype.rotateMain = function () {
    this.resetCanvas();

    var mc = this.current,
        nc = this.novellaCanvas,
        info = getImgInfo(nc.src),
        dw = info.width,
        dh = info.height,
        dx = (mc.width - dw) / 2,
        dy = (mc.height - dh) / 2;

    mc.context.save();
    mc.context.translate(mc.halfWidth, mc.halfHeight);
    mc.context.rotate(nc.degree * Math.PI / 180);
    mc.context.translate(-mc.halfWidth, -mc.halfHeight);
    mc.context.drawImage(nc.src, dx, dy, info.width, info.height);
    mc.context.restore();
};


canvasRender.prototype.flipCanvasHorizontal = function () {
    if (this.needsToFlipX) {
        this.current.context.scale(-1, 1);
        this.current.context.translate(-this.current.width, 0);
        this.needsToFlipX = false;
        this.novellaCanvas.needsToFlipX = false;
    }
};

canvasRender.prototype.flipCanvasVertical = function () {
    if (this.needsToFlipY) {
        this.current.context.scale(1, -1);
        this.current.context.translate(0, -this.current.height);
        this.needsToFlipY = false;
        this.novellaCanvas.needsToFlipY = false;
    }
};


//************CANVAS RENDER OBJECT*************************//
