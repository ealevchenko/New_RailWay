//-------------------------------------------------------
// Для работы с модулем добавте файл common.js
//-------------------------------------------------------

//---------------------------------------------
// ReferenceCountry
//---------------------------------------------
// Получить список справочника Country
function getAsyncReferenceCountry(callback) {
    $.ajax({
        type: 'GET',
        url: '/railway/api/rw/reference/country/all',
        async: true,
        dataType: 'json',
        beforeSend: function () {
            AJAXBeforeSend();
        },
        success: function (data) {
            if (typeof callback === 'function') {
                callback(data);
            }
        },
        error: function (x, y, z) {
            OnAJAXError(x, y, z);
        },
        complete: function () {
            AJAXComplete();
        },
    });
}
//---------------------------------------------
// ReferenceCargo
//---------------------------------------------
// Получить список справочника Cargo
function getAsyncReferenceCargo(callback) {
    $.ajax({
        type: 'GET',
        url: '/railway/api/rw/reference/cargo/all',
        async: true,
        dataType: 'json',
        beforeSend: function () {
            AJAXBeforeSend();
        },
        success: function (data) {
            if (typeof callback === 'function') {
                callback(data);
            }
        },
        error: function (x, y, z) {
            OnAJAXError(x, y, z);
        },
        complete: function () {
            AJAXComplete();
        },
    });
}
//---------------------------------------------
// ReferenceConsignee
//---------------------------------------------
// Получить список справочника Consignee
function getAsyncReferenceConsignee(callback) {
    $.ajax({
        type: 'GET',
        url: '/railway/api/rw/reference/consignee/all',
        async: true,
        dataType: 'json',
        beforeSend: function () {
            AJAXBeforeSend();
        },
        success: function (data) {
            if (typeof callback === 'function') {
                callback(data);
            }
        },
        error: function (x, y, z) {
            OnAJAXError(x, y, z);
        },
        complete: function () {
            AJAXComplete();
        },
    });
}
//---------------------------------------------
// ReferenceStation
//---------------------------------------------
// Получить список справочника Station
function getAsyncReferenceStation(callback) {
    $.ajax({
        type: 'GET',
        url: '/railway/api/rw/reference/station/all',
        async: true,
        dataType: 'json',
        beforeSend: function () {
            AJAXBeforeSend();
        },
        success: function (data) {
            if (typeof callback === 'function') {
                callback(data);
            }
        },
        error: function (x, y, z) {
            OnAJAXError(x, y, z);
        },
        complete: function () {
            AJAXComplete();
        },
    });
}
