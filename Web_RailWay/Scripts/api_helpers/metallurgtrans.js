//-------------------------------------------------------
// Для работы с модулем добавте файл common.js
//-------------------------------------------------------
// Веруть список всех вагонов за которыми установлено слижение
function getAsyncWagonsTracking(callback) {
    $.ajax({
        type: 'GET',
        url: '/railway/api/mt/wagons_tracking/',
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

// Веруть список составов на УЗ КР по которым есть не принятые вагоны
function getNoCloseArrivalCarsOfStationUZ(code, callback) {
    $.ajax({
        type: 'GET',
        url: '/railway/api/mt/arrival/cars/station_uz/'+code,
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

// Получить список вагонов по указанному прибытию
function getAsyncCarsOfArrivalUZ(id_sostav, is_close, callback) {
    $.ajax({
        type: 'GET',
        url: '/railway/api/mt/arrival/cars/sostav/' + id_sostav + '/close/'+is_close.toString(),
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