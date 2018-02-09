//-------------------------------------------------------
// Для работы с модулем добавте файл common.js
//-------------------------------------------------------
// Получить список путей по указанной станции
function getAsyncWaysStation(id_station, rospusk, callback) {
    $.ajax({
        type: 'GET',
        url: '/railway/api/rc/ways/station/' + id_station + '/rospusk/' + Boolean(rospusk).toString(),
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
// Получить спиок цехов по указанной станции
function getAsyncShopStation(id_station, rospusk, callback) {
    $.ajax({
        type: 'GET',
        url: '/railway/api/rc/shops/station/' + id_station + '/cars',
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
// Получить спиок цехов по указанной станции
function getAsyncWagonOverturnsStation(id_station, rospusk, callback) {
    $.ajax({
        type: 'GET',
        url: '/railway/api/rc/wagon_overturns/station/' + id_station + '/cars',
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
// Получить список вагонов на указанном пути
function getAsyncCarsOfWay(id_way, side, callback) {
    $.ajax({
        type: 'GET',
        url: '/railway/api/rc/cars/way/' + id_way + '/side/' + side,
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

// Получить список вагонов по указаному вагоноопрокиду
function getAsyncCarsOfWagonOverturn(id_gruz_front, callback) {
    $.ajax({
        type: 'GET',
        url: '/railway/api/rc/cars/wagon_overturn/' + id_gruz_front,
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

// Получить список вагонов по указанному цеху
function getAsyncCarsOfShop(id_shop, callback) {
    $.ajax({
        type: 'GET',
        url: '/railway/api/rc/cars/shop/' + id_shop,
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

// Получить список цехов по указанной станции
function getAsyncShopsOfStation(id_station, callback) {
    $.ajax({
        type: 'GET',
        url: '/railway/api/rc/shops/station/' + id_station,
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
// Получить список вагоноопрокидов по указанной станции
function getAsyncWagonOverturnsOfStation(id_station, callback) {
    $.ajax({
        type: 'GET',
        url: '/railway/api/rc/wagon_overturns/station/' + id_station,
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