/*
 * jQuery File Upload Plugin JS Example 8.9.1
 * https://github.com/blueimp/jQuery-File-Upload
 *
 * Copyright 2010, Sebastian Tschan
 * https://blueimp.net
 *
 * Licensed under the MIT license:
 * http://www.opensource.org/licenses/MIT
 */

/* global $, window */


$(function () {
    'use strict';

    // Initialize the jQuery File Upload widget:
    $('#fileupload').fileupload();

    // Load existing files:
    $.getJSON($('#fileupload form').prop('action'), function (files) {
        var fu = $('#fileupload').data('fileupload');
        fu._adjustMaxNumberOfFiles(-files.length);
        fu._renderDownload(files)
            .appendTo($('#fileupload .files'))
            .fadeIn(function () {
                // Fix for IE7 and lower:
                $(this).show();
            });
    });

    $('#fileupload').addClass('fileupload-processing');
        $.ajax({
            // Uncomment the following to send cross-domain cookies:
            //xhrFields: {withCredentials: true},
            url: $('#fileupload').fileupload('option', 'url'),
            dataType: 'json',
            context: $('#fileupload')[0],
            maxChunkSize: 500000//5000000000
        }).always(function () {
            $(this).removeClass('fileupload-processing');
        }).done(function (result) {
            $(this).fileupload('option', 'done')
                .call(this, $.Event('done'), { result: result });
        });
});

//$(function () {
//    'use strict';

//    // Initialize the jQuery File Upload widget:
//    $('#fileupload').fileupload({
//        // Uncomment the following to send cross-domain cookies:
//        //xhrFields: {withCredentials: true},
//        url: 'server/asp_net/UploadHandler_4_0.aspx',
//        maxChunkSize: 10000000
//    });

//    // Enable iframe cross-domain access via redirect option:
//    $('#fileupload').fileupload(
//        { maxChunkSize: 10000000 },
//        'option',
//        'redirect',
//        window.location.href.replace(
//            /\/[^\/]*$/,
//            '/cors/result.html?%s'
//        )
//    );

//    if (window.location.hostname === 'blueimp.github.io') {
//        // Demo settings:
//        $('#fileupload').fileupload('option', {
//            url: '//jquery-file-upload.appspot.com/',
//            // Enable image resizing, except for Android and Opera,
//            // which actually support image resizing, but fail to
//            // send Blob objects via XHR requests:
//            disableImageResize: /Android(?!.*Chrome)|Opera/
//                .test(window.navigator.userAgent),
//            maxChunkSize: 10000000,
//            acceptFileTypes: /(\.|\/)(gif|jpe?g|png)$/i
//        });
//        // Upload server status check for browsers with CORS support:
//        if ($.support.cors) {
//            $.ajax({
//                url: '//jquery-file-upload.appspot.com/',
//                type: 'HEAD'
//            }).fail(function () {
//                $('<div class="alert alert-danger"/>')
//                    .text('Upload server currently unavailable - ' +
//                            new Date())
//                    .appendTo('#fileupload');
//            });
//        }
//    } else {
//        // Load existing files:
//        $('#fileupload').addClass('fileupload-processing');
//        $.ajax({
//            // Uncomment the following to send cross-domain cookies:
//            //xhrFields: {withCredentials: true},
//            url: $('#fileupload').fileupload('option', 'url'),
//            dataType: 'json',
//            context: $('#fileupload')[0],
//            maxChunkSize: 10000000
//        }).always(function () {
//            $(this).removeClass('fileupload-processing');
//        }).done(function (result) {
//            $(this).fileupload('option', 'done')
//                .call(this, $.Event('done'), { result: result });
//        });
//    }

//});

//$(function () {
//    'use strict';

//    // Initialize the jQuery File Upload widget:
//    $('#fileupload').fileupload({
//        // Uncomment the following to send cross-domain cookies:
//        //xhrFields: {withCredentials: true},
//        url: 'server/asp_net/UploadHandler_4_0.aspx'
//    });

//    // Enable iframe cross-domain access via redirect option:
//    $('#fileupload').fileupload(
//        'option',
//        'redirect',
//        window.location.href.replace(
//            /\/[^\/]*$/,
//            '/cors/result.html?%s'
//        )
//    );

//    // Load existing files:
//    $('#fileupload').addClass('fileupload-processing');
//    $.ajax({
//        // Uncomment the following to send cross-domain cookies:
//        //xhrFields: {withCredentials: true},
//        url: $('#fileupload').fileupload('option', 'url'),
//        dataType: 'json',
//        context: $('#fileupload')[0],
//        maxFileSize: 500000000000
//    }).always(function () {
//        $(this).removeClass('fileupload-processing');
//    }).done(function (result) {
//        $(this).fileupload('option', 'done')
//            .call(this, $.Event('done'), { result: result });
//    });
//});
