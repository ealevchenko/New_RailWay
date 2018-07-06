//-------------------------------------------------------
// Для работы с модулем добавте файл common.js
//-------------------------------------------------------

//----------------------------------------------------------------
// KOMETA
//----------------------------------------------------------------
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
//
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
// Вернуть перечень собственников
function getAsyncKometaSobstvForNakl(callback) {
    $.ajax({
        type: 'GET',
        url: '/railway/api/kis/kometa/sobstv_for_nakl',
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
// Вернуть перечень стран
function getAsyncKometaStrana(callback) {
    $.ajax({
        type: 'GET',
        url: '/railway/api/kis/kometa/strana',
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
//----------------------------------------------------------------
// NUM_VAG
//----------------------------------------------------------------
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
//----------------------------------------------------------------
// PROM
//----------------------------------------------------------------
// Вернуть перечень входящих и исходящих составов
function getAsyncPromSostav(start, stop, callback) {
    $.ajax({
        type: 'GET',
        url: '/railway/api/kis/prom/sostav/start/' + start.toISOString() + '/stop/' + stop.toISOString(),
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
// Вернуть перечень входящих и исходящих составов по натурному листу и дате
function getAsyncPromSostavOfNaturAndDT(natur, day, month, year, hour, minute, callback) {
    $.ajax({
        type: 'GET',
        url: '/railway/api/kis/prom/sostav/natur/' + (natur != null ? natur : -1) +
            '/day/' + (day != null ? day : -1) +
            '/month/' + (month != null ? month : -1) +
            '/year/' + (year != null ? year : -1) +
            '/hour/' + (hour != null ? hour : -1) +            
            '/minute/' + (minute != null ? minute : -1),
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

// Вернуть список входящих вагонов таблица PROM.VAGON
function getAsyncArrivalPromVagon(natur, day, month, year, hour, minute, callback) {
    $.ajax({
        type: 'GET',
        url: '/railway/api/kis/prom/vagon/arrival/natur/' + natur + '/day/' + day + '/month/' + month + '/year/' + year + '/hour/' + hour + '/minute/' + minute,
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
// Вернуть список исходящих вагонов таблица PROM.VAGON
function getAsyncSendingPromVagon(natur, day, month, year, hour, minute, callback) {
    $.ajax({
        type: 'GET',
        url: '/railway/api/kis/prom/vagon/sending/natur/' + natur + '/day/' + day + '/month/' + month + '/year/' + year + '/hour/' + hour + '/minute/' + minute,
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
// Вернуть список вход и исход вагонов из таблицы PROM.VAGON по указаному вагону 
function getAsyncPromVagonAndSostav(num, callback) {
    $.ajax({
        type: 'GET',
        url: '/railway/api/kis/prom/vagon_sostav/num/' + num,
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

// Вернуть список входящих вагонов таблица PROM.NatHist
function getAsyncArrivalPromNatHist(natur, day, month, year, hour, minute, callback) {
    $.ajax({
        type: 'GET',
        url: '/railway/api/kis/prom/nat_hist/arrival/natur/' + natur + '/day/' + day + '/month/' + month + '/year/' + year + '/hour/' + hour + '/minute/' + minute,
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
// Вернуть список исходящих вагонов таблица PROM.NatHist
function getAsyncSendingPromNatHist(natur, day, month, year, hour, minute, callback) {
    $.ajax({
        type: 'GET',
        url: '/railway/api/kis/prom/nat_hist/sending/natur/' + natur + '/day/' + day + '/month/' + month + '/year/' + year + '/hour/' + hour + '/minute/' + minute,
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
// Вернуть список вход и исход вагонов из таблицы PROM.NatHist по указаному вагону 
function getAsyncPromNatHistAndSostav(num, callback) {
    $.ajax({
        type: 'GET',
        url: '/railway/api/kis/prom/nathist_sostav/num/' + num,
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

// Вернуть перечень цехов получателей грузов
function getAsyncPromCex(callback) {
    $.ajax({
        type: 'GET',
        url: '/railway/api/kis/prom/cex',
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
// Вернуть перечень грузов
function getAsyncGruzSP(callback) {
    $.ajax({
        type: 'GET',
        url: '/railway/api/kis/prom/gruz_sp',
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
