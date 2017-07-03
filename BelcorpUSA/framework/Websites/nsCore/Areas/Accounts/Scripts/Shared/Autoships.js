function checkDuplicateUrlsForUser(isSubdomain) {
    var result = true;
    var data = new Array();
    var url = '';
    $('#siteUrls .urlContainer').each(function (i) {
        $(this).clearError();
        if ($('.toggleCustomUrl', this).prop('checked')) {
            url = 'http://' + $('.url', this).val();
        } else {
            if (parseBool(isSubdomain)) {
                url = 'http://' + $('.subdomain', this).val() + '.' + $('.domain', this).val();
            } else {
                url = 'http://www.' + $('.domain', this).val() + '/' + $('.subdomain', this).val();
            }
        }

        if (isUrlUnique(data, url)) {
            data[i] = url;
        }
        else {
            $(this).showError('');
            result = false;
        }
    });

    return result;
}

function isUrlUnique(data, url) {
    var result = true;
    $(data).each(function (i) {
        if (this != '' && url != '' && this.toUpperCase() == url.toUpperCase()) {
            result = false;
        }
    });

    return result;
}


function populateSiteURLList(data, isSubdomain) {
    $('#siteUrls .urlContainer').each(function(i) {
        data['urls[' + i + '].SiteUrlID'] = $('.siteUrlId', this).length ? $('.siteUrlId', this).val() : 0;
        if ($('.toggleCustomUrl', this).prop('checked')) {
            data['urls[' + i + '].Url'] = 'http://' + $('.url', this).val();
        } else {
            if (parseBool(isSubdomain)) {
                data['urls[' + i + '].Url'] = 'http://' + $('.subdomain', this).val() + '.' + $('.domain', this).val();
            } else {
                data['urls[' + i + '].Url'] = 'http://www.' + $('.domain', this).val() + '/' + $('.subdomain', this).val();
            }
        }
    });
}

function checkForEmptyUrls() {
    var urlTextBox = $('.urlContainer .fixedDomain input.subdomain');
    var result = true;
    urlTextBox.each(function (i) {
        if ($(this).val().length === 0) {
            $(this).parent().parent().showError('');
            result = false;
        }
    });
    return result;
}

