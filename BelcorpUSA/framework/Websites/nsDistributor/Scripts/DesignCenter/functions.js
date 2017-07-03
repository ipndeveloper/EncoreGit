function updateCanvas(currentLi, currentCanvas) {
    if (currentCanvas == null || typeof currentCanvas == "undefined")
        return false;

    var myCanvas = currentCanvas,
        index = myCanvas.index,
        frame = currentLi.data('frame'),
        mainImage = currentLi.find('input:hidden[name="MainPreviewPath"]').val() || "",
        isEditing = currentLi.find('input:hidden[name="IsEditing"]').val();

    myCanvas.showAsset = false;
    if (mainImage !== "") {
        if (myCanvas.hasBeenLoaded !== true) {
            myCanvas.hasBeenLoaded = true;

            if (isEditing === "False") {
                myCanvas.isApproved = true;
                myCanvas.isEditing = false;
            } else {
                myCanvas.isApproved = false;
                myCanvas.isEditing = true;
            }

            createCanvasAppendTo('MainCanvas' + myCanvas.index, 'SelectedImage');
            createCanvasAppendTo('PreviewCanvas' + myCanvas.index, 'PreviewWrapper');
            myCanvas.addImageDraw(mainImage, index);
            mainButtonsUI(false);
        }

        if (currentLi.children('img[id^="previewAsset"]').length > 0) {
            if (myCanvas.isApproved) {
                var $previewImage = currentLi.children('img[id^="previewAsset"]').clone();
                $previewImage.height(frame.height).width(frame.width);
                $('#PreviewFrame').after($previewImage);
            }
            myCanvas.showAsset = true;
        }
    }
    replaceCanvasData(myCanvas);
    return false;
}

function replaceCanvasData(updatedNovellaCanvas) {
    if (typeof updatedNovellaCanvas !== "undefined") {
        $('ul.designItems li.selected').data('novellaCanvas', updatedNovellaCanvas);
    }
}

function createNewCanvasToSave(newCanvasId, newCanvasWidth, newCanvasHeight) {
    var newCanvas = document.createElement('canvas');
    ensureCanvas(newCanvas);
    newCanvas.width = newCanvasWidth;
    newCanvas.height = newCanvasHeight;
    if (newCanvasId !== "" || typeof newCanvasId !== "undefined") {
        $(newCanvas).attr('id', newCanvasId);
    }
    return newCanvas;
}

function saveIsApprovedFlag(isDesignApproved) {
    var bundleId = $('#BundleID').val(),
        liCount = $('#novellaDesignItems_UL li').length,
        isNotBundle = (bundleId === null || bundleId === "");

    if (liCount > 1 && isNotBundle) {
        $.post('/Design/SaveApprovedFlagMultiple', { allApproved: allNovellasHasPreviews(), parentId: $('#OrderItemID').val() || $('#ProductID').val() }, function () {
            MyCurrentCanvasCallBacks.rc.requestComplete(true);
        });
    }
    else if (isNotBundle) {
        $.post('/Design/SaveApprovedSingleNovella', { allApproved: isDesignApproved, OrderItem: null, orderItemId: $('#novellaDesignItems_UL li.selected').attr('id') },
        function () {
            MyCurrentCanvasCallBacks.rc.requestComplete(true);
        });

    } else {
        $.post('/Design/SaveApprovedFlagMultiple', { allApproved: allNovellasHasPreviews(), bundleGuid: $('#BundleID').val() }, function () {
            MyCurrentCanvasCallBacks.rc.requestComplete(true);
        });
    }
}

function getMainCanvasSize(imgInfo, canvasInfo) {
    var canvasWidth, canvasHeight, offsetW = 0, offsetH = 0;

    switch (Math.abs(canvasInfo.degree)) {
        case 0:
        case 180:
            canvasHeight = imgInfo.height;
            canvasWidth = imgInfo.width;
            offsetW = (imgInfo.canvasWidth - imgInfo.width) / 2;
            offsetH = (imgInfo.canvasHeight - imgInfo.height) / 2;
            break;
        case 90:
        case 270:
            canvasHeight = imgInfo.width;
            canvasWidth = imgInfo.height;
            offsetH = (imgInfo.canvasWidth - imgInfo.width) / 2;
            offsetW = (imgInfo.canvasHeight - imgInfo.height) / 2;
            break;
        default:
            canvasWidth = "";
            canvasHeight = "";
            break;
    }
    return { dw: canvasWidth, dh: canvasHeight, dx: offsetW, dy: offsetH };
}

function SaveIsShareable(isSharable) {
    var selected = $('#novellaDesignItems_UL li.selected');
    if (typeof selected !== "undefined" && selected != null) {
        $.post('/Design/SaveIsShareable', { isShareable: isSharable, bundleGuid: $('#BundleID').val(), orderItemGuid: selected.attr('id') });
    }
}

function SaveCanvasImage(canvasId, propName, orderItemGuid, actualCanvas) {
    var canvas = document.getElementById(canvasId) || actualCanvas;
    if (typeof canvas != 'undefined' && canvas != null) {
//        var canvasData = canvas.toDataURL("image/png").replace('data:image/png;base64,', '');
//        var canvasData = canvas.toDataURL("image/jpeg").replace('data:image/jpeg;base64,', '');
        var canvasData = canvasToImage(canvas, "#FFFFFF");
        
        $.ajax({
            type: "POST",
            dataType: "json",
            data: { baseString: canvasData, propName: propName, orderItemGuid: orderItemGuid },

            beforeSend: function (x) {
                if (x && x.overrideMimeType) {
                    x.overrideMimeType("application/json;charset=UTF-8");
                }
            },
            url: '/Design/SavePreviewToOrder',
            complete: function () {
                $('#PleaseWaitWhileWeUploadYourFiles').hide();
                $('#PleaseWaitWhileWeSaveYourOrder').show();
                MyCurrentCanvasCallBacks.rc.requestComplete(true);
            }
        });
    }
}

function canvasToImage(canvas, backgroundColor) {
    //cache height and width		
    var w = canvas.width;
    var h = canvas.height;
    var context = canvas.getContext("2d");
    //get the current ImageData for the canvas.
    var data = context.getImageData(0, 0, w, h);
    //store the current globalCompositeOperation
    var compositeOperation = context.globalCompositeOperation;
    
    if (backgroundColor) {
         //set to draw behind current content
        context.globalCompositeOperation = "destination-over";

        //set background color
        context.fillStyle = backgroundColor;

        //draw background / rect on entire canvas
        context.fillRect(0, 0, w, h);
    }

    //get the image data from the canvas
    var imageData = canvas.toDataURL("image/jpeg").replace('data:image/jpeg;base64,', '');

    if (backgroundColor) {
        //clear the canvas
        context.clearRect(0, 0, w, h);

        //restore it with original / cached ImageData
        context.putImageData(data, 0, 0);

        //reset the globalCompositeOperation to what it was
        context.globalCompositeOperation = compositeOperation;
    }

    //return the Base64 encoded data url string
    return imageData;
}

function showLoggedIn() {
    if ($("#recentImagesPlaceHolder").length > 0) {
        $.post('/Design/Recent', { action: "get" }, function (data) {
            $('#recentImagesPlaceHolder').replaceWith(data);
            $('div.designWindow').removeClass('brdrAll').addClass('brdrNNYY');
            $('#ViewLibrary').show();
        });
    }
}

function refreshRecent() {
    $('#recentLibraryContainer').empty();
    $.post('/Design/Recent', { action: "get" }, function (data) {
        $('#recentLibraryContainer').html(data);
    });
}

function getUserGalleryLibrary() {
    $.post('/Design/Gallery', { action: "get" }, function (data) {
        $('#mediaLibrary').empty();
        $('#mediaLibrary').html('<div class="mContent">' + data + '</div>');
    });
}

function onLoggedIn() {
    showLoggedIn();
    $('#BrowseComputer').click(function () {
        $('div.selectImageText').hide();
    });

    $('#btnLogin').ajaxSuccess(function (evt, request, settings) {
        if (settings.url == '/Login') {
            var responseObj = JSON.parse(request.responseText);
            if (responseObj.result) {
                $('#LoginPrompt').jqmHide();
                $('.jqmOverlay').jqmHide();
                showLoggedIn();
            }
        }
    });
}

function canvasBeginProcessing() {
    mainButtonsUI(false);
    $('#PleaseWaitWhileWeManipulateTheCanvas').show();
    $('#savingNovellasInProgress').jqmShow();
    MyCurrentCanvasCallBacks.isProcessing = true;
}

function canvasEndProcessing() {
    MyCurrentCanvasCallBacks.isProcessing = false;
    mainButtonsUI(true);
    $('#savingNovellasInProgress div').children().hide();
    $('#savingNovellasInProgress').jqmHide();
}

function canvasIsProcessing() {
    return MyCurrentCanvasCallBacks.isProcessing;
}


//removes -deactive suffix from class
function activateClass() {
    var element = $(this), currentClass = element.attr('class'), replacementClass = currentClass.replace('-deactive', '');
    element.attr('class', replacementClass);
}

function copyNovellaCanvas(canvasToCopy) {
    var tempInfo = new novellaCanvas(canvasToCopy.isFlipX, canvasToCopy.isFlipY, false, canvasToCopy.degree, 1, canvasToCopy.index);
    tempInfo.position = canvasToCopy.position;
    tempInfo.src = canvasToCopy.src;
    tempInfo.lightenVal = canvasToCopy.lightenVal;
    tempInfo.isDesat = canvasToCopy.isDesat;
    return tempInfo;
}


function SaveFulFillment(myCanvas, frame, imgInfo, selectedLi) {
    var tempInfo = copyNovellaCanvas(myCanvas),
    originalWidth = frame.width / myCanvas.scale,
    originalHeight = frame.height / myCanvas.scale;

    var fulfillmentAsset = createNewCanvasToSave('', originalWidth, originalHeight),
    contextfulfillmentAsset = fulfillmentAsset.getContext('2d');

    var current = {
        context: contextfulfillmentAsset,
        halfWidth: originalWidth / 2,
        halfHeight: originalHeight / 2,
        width: originalWidth,
        height: originalHeight
    };

    var render = new canvasRender(null, tempInfo, null, current);
    render.drawAsset(false);

    SaveCanvasImage('', 'ModifiedMainImageFilePath', selectedLi.attr("id"), fulfillmentAsset);

    var minfo = getMainCanvasSize(imgInfo, myCanvas);
    var mainCanvasAsset;
    //any kind of rotation save as is
    if (minfo.dw === "") {
        mainCanvasAsset = createNewCanvasToSave('', imgInfo.canvasWidth, imgInfo.canvasHeight);
    } else {
        //upright or side crop unecessary edges
        mainCanvasAsset = createNewCanvasToSave('', minfo.dw, minfo.dh);
    }
    var mainCanvasAssetContext = mainCanvasAsset.getContext('2d');
    render = new canvasRender(null, tempInfo, null, {
        context: mainCanvasAssetContext,
        halfWidth: mainCanvasAsset.width / 2,
        halfHeight: mainCanvasAsset.height / 2,
        width: mainCanvasAsset.width,
        height: mainCanvasAsset.height
    });
    render.drawAsset(true);
    SaveCanvasImage('', "MainCanvasFilePath", selectedLi.attr("id"), mainCanvasAsset);
}


function ShrinkPreviewImageSize(p) {
    $('div.designItems img').each(function () {
        $(this).width(Math.floor(this.width * p));
    });
}

function findYetUnapprovedDesigns() {
    var result = [];
    $('ul.designItems li').each(function () {
        if ($(this).data('novellaCanvas').isApproved != true && $(this).children('img').length <= 1) {
            result.push($(this).attr('id'));
        }
    });
    return result;
}

function selectNextUnapprove() {
    var arr = findYetUnapprovedDesigns();
    if (arr.length > 0) {
        $('ul.designItems li').removeClass('selected');
        var id = '#' + arr[0];
        $(id).addClass('selected');
        $(id).trigger('click');
    } else {
        mainButtonsUI(false);
    }
}

var MyRequestsCompleted = (function () {
    var numRequestToComplete,
        requestsCompleted,
        callBacks,
        singleCallBack;

    return function (options) {
        if (!options) options = {};

        numRequestToComplete = options.numRequest || 0;
        requestsCompleted = options.requestsCompleted || 0;
        callBacks = [];
        var fireCallbacks = function () {
            for (var i = 0; i < callBacks.length; i++) callBacks[i]();
        };
        if (options.singleCallBack) callBacks.push(options.singleCallBack);

        this.addCallbackToQueue = function (isComplete, callback) {
            if (isComplete) requestsCompleted++;
            if (callback) callBacks.push(callback);
            if (requestsCompleted == numRequestToComplete) fireCallbacks();
        };
        this.requestComplete = function (isComplete) {
            if (isComplete) requestsCompleted++;
            if (requestsCompleted == numRequestToComplete) fireCallbacks();
        };
        this.setCallback = function (callback) {
            callBacks.push(callback);
        };
    };
})();

