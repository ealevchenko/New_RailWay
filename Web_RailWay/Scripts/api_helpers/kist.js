//-------------------------------------------------------
// Для работы с модулем добавте файл common.js
//-------------------------------------------------------
// Получить статус строки
function getStatus(status) {
    var value = "?";  //значение по умолчанию умолчанию
    $.ajax({
        type: 'GET',
        url: status != null ? '/railway/api/kis/transfer/status/' + status : '/railway/api/kis/transfer/status',
        async: false,
        dataType: 'json',
        success: function (data) {
            value = data;
        }
    });
    return value;
}

function getAsyncStatus(status, callback) {
    $.ajax({
        type: 'GET',
        url: status != null ? '/railway/api/kis/transfer/status/' + status : '/railway/api/kis/transfer/status',
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
// Получить статус строки
function getStatusName(status) {
    var value = "?";  //значение по умолчанию умолчанию
    $.ajax({
        type: 'GET',
        url: status != null ? '/railway/api/kis/transfer/status/' + status + '/name' : '/railway/api/kis/transfer/status/name',
        async: false,
        dataType: 'json',
        success: function (data) {
            value = data;
        }
    });
    return value;
}
function getAsyncStatusName(status, callback) {
    $.ajax({
        type: 'GET',
        url: status != null ? '/railway/api/kis/transfer/status/' + status + '/name' : '/railway/api/kis/transfer/status/name',
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

// Получить статус строки
function getError(err) {
    var value = "?";  //значение по умолчанию умолчанию
    $.ajax({
        type: 'GET',
        url: err != null ? '/railway/api/kis/transfer/error/' + err : '/railway/api/kis/transfer/error',
        async: false,
        dataType: 'json',
        success: function (data) {
            value = data;
        }
    });
    return value;
}

function getStatusCars(list, car) {
    if (list != null) {
        if (list.indexOf(car) >= 0) {
            return 'Error'
        }
    }
    return 'Ok'
}

function getErrorCars(list_error, car) {
    if (list_error != null) {
        var cars_err = list_error.split(';');
        for (var i = 0; i < cars_err.length; i++) {
            car_arr = cars_err[i].split(':');
            if (car_arr[0] == car) {
                return car_arr[1];
            }
        }
    }
    return "Ok";
}

// получить статус переноса вагонов 
function getBufferArrivalCarsStatus(bac) {
    var list_err = getError();
    if (bac != null) {
        var list_cars = [];
        if (bac.list_wagons != null) {
            var cars = bac.list_wagons.split(';');
            for (var i = 0; i < cars.length; i++) {
                var upd_err = getErrorCars(bac.message, cars[i]);
                if (cars[i] != "") {
                    list_cars.push({
                        car: cars[i],
                        car_set: getStatusCars(bac.list_no_set_wagons, cars[i]),
                        car_upd: getStatusCars(bac.list_no_update_wagons, cars[i]),
                        car_upderr: (upd_err != "Ok" ? getTextOption(list_err, upd_err) : upd_err),
                    });
                }
            }
        }
        return list_cars;
    }
}
// Получить список строк BufferArrivalSostav за указанное время
function getBufferArrivalSostav(correct, start, stop) {
    //OnBegin();
    var value = null;  //значение по умолчанию умолчанию
    var list_kometa_stan = getKometaStationName();
    var list_status = getStatus();
    var list_status_name = getStatusName();
    $.ajax({
        url: '/railway/api/kis/transfer/bas/' + start.toISOString() + '/' + stop.toISOString(),
        type: 'GET',
        async: false,
        dataType: 'json',
        beforeSend: function () {
            AJAXBeforeSend();
        },
        success: function (jsondata) {
            if (correct) {
                var list_log = [];
                for (var i = 0; i < jsondata.length; i++) {
                    list_log.push({
                        id: jsondata[i].id,
                        datetime: jsondata[i].datetime != null ? jsondata[i].datetime.replace("T", " ") : null,
                        day: jsondata[i].day,
                        month: jsondata[i].month,
                        year: jsondata[i].year,
                        hour: jsondata[i].hour,
                        minute: jsondata[i].minute,
                        natur: jsondata[i].natur,
                        id_station_kis: jsondata[i].id_station_kis,
                        station_name: getTextOption(list_kometa_stan, jsondata[i].id_station_kis),
                        way_num: jsondata[i].way_num,
                        napr: jsondata[i].napr,
                        count_wagons: jsondata[i].count_wagons,
                        count_nathist: jsondata[i].count_nathist,
                        count_set_wagons: jsondata[i].count_set_wagons,
                        count_set_nathist: jsondata[i].count_set_nathist,
                        close: jsondata[i].close != null ? jsondata[i].close.replace("T", " ") : null,
                        close_user: jsondata[i].close_user,
                        status: getTextOption(list_status, jsondata[i].status),
                        statusname: getTextOption(list_status_name, jsondata[i].status),

                        list_wagons: jsondata[i].list_wagons,
                        list_no_set_wagons: jsondata[i].list_no_set_wagons,
                        list_no_update_wagons: jsondata[i].list_no_update_wagons,
                        message: jsondata[i].message,

                    });
                }
            }
            value = list_log;
        },
        error: function (x, y, z) {
            OnAJAXError(x, y, z);
        },
        complete: function () {
            AJAXComplete();
        },
    });
    //LockScreenOff();
    return value;
}

function getCorrectBufferArrivalSostav(jsondata, list_kometa_stan, list_status, list_status_name) {
    var list_log = [];
    for (var i = 0; i < jsondata.length; i++) {
        list_log.push({
            id: jsondata[i].id,
            datetime: jsondata[i].datetime != null ? jsondata[i].datetime.replace("T", " ") : null,
            day: jsondata[i].day,
            month: jsondata[i].month,
            year: jsondata[i].year,
            hour: jsondata[i].hour,
            minute: jsondata[i].minute,
            natur: jsondata[i].natur,
            id_station_kis: jsondata[i].id_station_kis,
            station_name: getTextOption(list_kometa_stan, jsondata[i].id_station_kis),
            way_num: jsondata[i].way_num,
            napr: jsondata[i].napr,
            count_wagons: jsondata[i].count_wagons,
            count_nathist: jsondata[i].count_nathist,
            count_set_wagons: jsondata[i].count_set_wagons,
            count_set_nathist: jsondata[i].count_set_nathist,
            close: jsondata[i].close != null ? jsondata[i].close.replace("T", " ") : null,
            close_user: jsondata[i].close_user,
            status: getTextOption(list_status, jsondata[i].status),
            statusname: getTextOption(list_status_name, jsondata[i].status),
            list_wagons: jsondata[i].list_wagons,
            list_no_set_wagons: jsondata[i].list_no_set_wagons,
            list_no_update_wagons: jsondata[i].list_no_update_wagons,
            message: jsondata[i].message,
        });
    }
    return list_log;
}

function getAsyncBufferArrivalSostav(start, stop, callback) {
    $.ajax({
        url: '/railway/api/kis/transfer/bas/' + start.toISOString() + '/' + stop.toISOString(),
        type: 'GET',
        async: true,
        dataType: 'json',
        beforeSend: function () {
            AJAXBeforeSend();
        },
        success: function (jsondata) {
            if (typeof callback === 'function') {
                callback(jsondata);
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

//----------------------------------------------------------------------------------------------------
// Получить список строк BufferInputSostav за указанное время
function getBufferInputSostav(correct, start, stop) {
    var value = null;  //значение по умолчанию умолчанию
    var list_numvag_stan = getNumVagStationName();

    var list_status = getStatus();
    var list_status_name = getStatusName();
    $.ajax({
        url: '/railway/api/kis/transfer/bis/' + start.toISOString() + '/' + stop.toISOString(),
        type: 'GET',
        async: false,
        dataType: 'json',
        beforeSend: function () {
            AJAXBeforeSend();
        },
        success: function (jsondata) {
            if (correct) {
                var list_log = [];
                for (var i = 0; i < jsondata.length; i++) {
                    list_log.push({
                        id: jsondata[i].id,
                        datetime: jsondata[i].datetime != null ? jsondata[i].datetime.replace("T", " ") : null,
                        doc_num: jsondata[i].doc_num,
                        id_station_from_kis: jsondata[i].id_station_from_kis,
                        station_from_name: getTextOption(list_numvag_stan, jsondata[i].id_station_from_kis),
                        way_num_kis: jsondata[i].way_num_kis,
                        napr: jsondata[i].napr,
                        id_station_on_kis: jsondata[i].id_station_on_kis,
                        station_on_name: getTextOption(list_numvag_stan, jsondata[i].id_station_on_kis),
                        count_wagons: jsondata[i].count_wagons,
                        count_set_wagons: jsondata[i].count_set_wagons,
                        natur: jsondata[i].natur,
                        close: jsondata[i].close != null ? jsondata[i].close.replace("T", " ") : null,
                        close_user: jsondata[i].close_user,
                        status: getTextOption(list_status, jsondata[i].status),
                        statusname: getTextOption(list_status_name, jsondata[i].status),
                        message: jsondata[i].message,
                    });
                }
            }
            value = list_log;
        },
        error: function (x, y, z) {
            OnAJAXError(x, y, z);
        },
        complete: function () {
            AJAXComplete();
        },
    });
    return value;
}
//
function getBufferInputSostavXMLHttpRequest(correct, start, stop, callback) {
    //var value = null;  //значение по умолчанию умолчанию
    var list_numvag_stan = getNumVagStationName();

    var list_status;// = getStatus();
    var list_status;//_name = getStatusName();
    var xhr = new XMLHttpRequest(),
    method = "GET",
    url = '/railway/api/kis/transfer/bis/' + start.toISOString() + '/' + stop.toISOString();

    xhr.open(method, url, true);
    xhr.onreadystatechange = function () {
        if (xhr.readyState === XMLHttpRequest.DONE && xhr.status === 200) {
            jsondata = JSON.parse(xhr.responseText);
            if (correct) {
                var list_log = [];
                for (var i = 0; i < jsondata.length; i++) {
                    list_log.push({
                        id: jsondata[i].id,
                        datetime: jsondata[i].datetime != null ? jsondata[i].datetime.replace("T", " ") : null,
                        doc_num: jsondata[i].doc_num,
                        id_station_from_kis: jsondata[i].id_station_from_kis,
                        station_from_name: getTextOption(list_numvag_stan, jsondata[i].id_station_from_kis),
                        way_num_kis: jsondata[i].way_num_kis,
                        napr: jsondata[i].napr,
                        id_station_on_kis: jsondata[i].id_station_on_kis,
                        station_on_name: getTextOption(list_numvag_stan, jsondata[i].id_station_on_kis),
                        count_wagons: jsondata[i].count_wagons,
                        count_set_wagons: jsondata[i].count_set_wagons,
                        natur: jsondata[i].natur,
                        close: jsondata[i].close != null ? jsondata[i].close.replace("T", " ") : null,
                        close_user: jsondata[i].close_user,
                        status: getTextOption(list_status, jsondata[i].status),
                        statusname: getTextOption(list_status_name, jsondata[i].status),
                        message: jsondata[i].message,
                    });
                }
            }
            if (typeof callback === 'function') {
                callback(list_log);

            }
        };
    };
    xhr.send();
}

function getCorrectBufferInputSostav(jsondata, list_numvag_stan, list_status, list_status_name) {
    var list_log = [];
    for (var i = 0; i < jsondata.length; i++) {
        list_log.push({
            id: jsondata[i].id,
            datetime: jsondata[i].datetime != null ? jsondata[i].datetime.replace("T", " ") : null,
            doc_num: jsondata[i].doc_num,
            id_station_from_kis: jsondata[i].id_station_from_kis,
            station_from_name: getTextOption(list_numvag_stan, jsondata[i].id_station_from_kis),
            way_num_kis: jsondata[i].way_num_kis,
            napr: jsondata[i].napr,
            id_station_on_kis: jsondata[i].id_station_on_kis,
            station_on_name: getTextOption(list_numvag_stan, jsondata[i].id_station_on_kis),
            count_wagons: jsondata[i].count_wagons,
            count_set_wagons: jsondata[i].count_set_wagons,
            natur: jsondata[i].natur,
            close: jsondata[i].close != null ? jsondata[i].close.replace("T", " ") : null,
            close_user: jsondata[i].close_user,
            status: getTextOption(list_status, jsondata[i].status),
            statusname: getTextOption(list_status_name, jsondata[i].status),
            message: jsondata[i].message,
        });
    }
    return list_log;
}

function getAsyncBufferInputSostav(start, stop, callback) {
    $.ajax({
        url: '/railway/api/kis/transfer/bis/' + start.toISOString() + '/' + stop.toISOString(),
        type: 'GET',
        async: true,
        dataType: 'json',
        beforeSend: function () {
            AJAXBeforeSend();
        },
        success: function (jsondata) {
            if (typeof callback === 'function') {
                callback(jsondata);
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
//-------------------------------------------------------------------------------------------------------
//
function getCorrectBufferOutputSostav(jsondata, list_numvag_stan, list_status, list_status_name) {
    var list_log = [];
    for (var i = 0; i < jsondata.length; i++) {
        list_log.push({
            id: jsondata[i].id,
            datetime: jsondata[i].datetime != null ? jsondata[i].datetime.replace("T", " ") : null,
            doc_num: jsondata[i].doc_num,
            id_station_on_kis: jsondata[i].id_station_on_kis,
            station_on_name: getTextOption(list_numvag_stan, jsondata[i].id_station_on_kis),
            way_num_kis: jsondata[i].way_num_kis,
            napr: jsondata[i].napr,
            id_station_from_kis: jsondata[i].id_station_from_kis,
            station_from_name: getTextOption(list_numvag_stan, jsondata[i].id_station_from_kis),
            count_wagons: jsondata[i].count_wagons,
            count_set_wagons: jsondata[i].count_set_wagons,
            close: jsondata[i].close != null ? jsondata[i].close.replace("T", " ") : null,
            close_user: jsondata[i].close_user,
            status: getTextOption(list_status, jsondata[i].status),
            statusname: getTextOption(list_status_name, jsondata[i].status),
            message: jsondata[i].message,
        });
    }
    return list_log;
}
//
function getAsyncBufferOutputSostav(start, stop, callback) {
    $.ajax({
        url: '/railway/api/kis/transfer/bos/' + start.toISOString() + '/' + stop.toISOString(),
        type: 'GET',
        async: true,
        dataType: 'json',
        beforeSend: function () {
            AJAXBeforeSend();
        },
        success: function (jsondata) {
            if (typeof callback === 'function') {
                callback(jsondata);
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

//--------------------------------------------------------------------------------------------------------
// RAILWAY
//--------------------------------------------------------------------------------------------------------
// Получить список строк RWBufferArrivalSostav за указанное время
function getAsyncRWBufferArrivalSostav(start, stop, callback) {
    $.ajax({
        url: '/railway/api/kis/transfer/arrival/' + start.toISOString() + '/' + stop.toISOString(),
        type: 'GET',
        async: true,
        dataType: 'json',
        beforeSend: function () {
            AJAXBeforeSend();
        },
        success: function (jsondata) {
            if (typeof callback === 'function') {
                callback(jsondata);
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
// Получить строку RWBufferArrivalSostav по id
function getAsyncRWBufferArrivalSostavOfID(id, callback) {
    $.ajax({
        url: '/railway/api/kis/transfer/arrival/id/' + id,
        type: 'GET',
        async: true,
        dataType: 'json',
        beforeSend: function () {
            AJAXBeforeSend();
        },
        success: function (jsondata) {
            if (typeof callback === 'function') {
                callback(jsondata);
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
// Получить список строк RWBufferSendingSostav за указанное время
function getAsyncRWBufferSendingSostav(start, stop, callback) {
    $.ajax({
        url: '/railway/api/kis/transfer/sending/' + start.toISOString() + '/' + stop.toISOString(),
        type: 'GET',
        async: true,
        dataType: 'json',
        beforeSend: function () {
            AJAXBeforeSend();
        },
        success: function (jsondata) {
            if (typeof callback === 'function') {
                callback(jsondata);
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
