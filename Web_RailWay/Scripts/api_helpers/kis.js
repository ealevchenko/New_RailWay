//-------------------------------------------------------
// Для работы с модулем добавте файл common.js
//-------------------------------------------------------
// Получить название станций из схемы Kometa
function getKometaStationName(id) {
    var value = "?";  //значение по умолчанию умолчанию
    $.ajax({
        type: 'GET',
        url: id !=null ? '/railway/api/kis/kometa/station/' + id + '/name' : '/railway/api/kis/kometa/station/name',
        async: false,
        dataType: 'json',
        success: function (data) {
            value = data;
        }
    });
    return value;
}

function getAsyncKometaStationName(id, callback) {
    $.ajax({
        type: 'GET',
        url: id !=null ? '/railway/api/kis/kometa/station/' + id + '/name' : '/railway/api/kis/kometa/station/name',
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

// Получить название станций из схемы NumVag
function getNumVagStationName(id) {
    var value = "?";  //значение по умолчанию умолчанию
    $.ajax({
        type: 'GET',
        url: id != null ? '/railway/api/kis/num_vag/station/' + id + '/name' : '/railway/api/kis/num_vag/station/name',
        async: false,
        dataType: 'json',
        success: function (data) {
            value = data;
        }
    });
    return value;
}

function getAsyncNumVagStationName(id, callback) {
    $.ajax({
        type: 'GET',
        url: id != null ? '/railway/api/kis/num_vag/station/' + id + '/name' : '/railway/api/kis/num_vag/station/name',
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
