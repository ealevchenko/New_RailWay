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


