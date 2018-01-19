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
        success: function (data) {
            if (typeof callback === 'function') {
                callback(data);
            }
        }
    });
}

// Получить название станций из схемы NumVag
function getNumVagStationName(id) {
    var value = "?";  //значение по умолчанию умолчанию
    $.ajax({
        type: 'GET',
        url: id != null ? '/api/kis/num_vag/station/' + id + '/name' : '/api/kis/num_vag/station/name',
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
        success: function (data) {
            if (typeof callback === 'function') {
                callback(data);
            }
        }
    });
}
