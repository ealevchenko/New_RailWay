// Получить список брака вработе
function getAsyncDTMarriage(callback) {
    $.ajax({
        type: 'GET',
        url: '../../api/dt/marriage_work',
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
        }
    });
}
// Получить список брака в работе за указаный период
function getAsyncDTMarriageOfDate(start, stop, callback) {
    $.ajax({
        type: 'GET',
        url: '../../api/dt/marriage_work/start/' + toISOStringTZ(start).substring(0, 19) + '/stop/' + toISOStringTZ(stop).substring(0, 19),
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
        }
    });
}
// Получить список брака в работе за указаный период
function getAsyncDTMarriageOfID(id, callback) {
    $.ajax({
        type: 'GET',
        url: '../../api/dt/marriage_work/id/' + id,
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
        }
    });
}
//Добавить брака вработе
var postAsyncDTMarriage = function (marriage_work, callback) {
    $.ajax({
        url: '../../api/dt/marriage_work',
        type: 'POST',
        data: JSON.stringify(marriage_work),
        contentType: "application/json;charset=utf-8",
        async: true,
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
//Обновить брака вработе
var putAsyncDTMarriage = function (marriage_work, callback) {
    $.ajax({
        type: 'PUT',
        url: '../../api/dt/marriage_work/' + marriage_work.id,
        data: JSON.stringify(marriage_work),
        contentType: "application/json;charset=utf-8",
        async: true,
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
// Удалить брака вработе по id
var deleteAsyncDTMarriage = function (id, callback) {
    $.ajax({
        url: '../../api/dt/marriage_work/' + id,
        type: 'DELETE',
        contentType: "application/json;charset=utf-8",
        async: true,
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


// Получить список MarriageDistrictObject
function getAsyncDTMarriageDistrictObject(callback) {
    $.ajax({
        type: 'GET',
        url: '../../api/dt/marriage_district_object',
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
        }
    });
}

// Получить список Classification
function getAsyncDTMarriageClassification(callback) {
    $.ajax({
        type: 'GET',
        url: '../../api/dt/marriage_classification',
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
        }
    });
}

// Получить список Naturen
function getAsyncDTMarriageNature(callback) {
    $.ajax({
        type: 'GET',
        url: '../../api/dt/marriage_nature',
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
        }
    });
}

// Получить список Cause
function getAsyncDTMarriageCause(callback) {
    $.ajax({
        type: 'GET',
        url: '../../api/dt/marriage_cause',
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
        }
    });
}

// Получить список Subdivision
function getAsyncDTMarriageSubdivision(callback) {
    $.ajax({
        type: 'GET',
        url: '../../api/dt/marriage_subdivision',
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
        }
    });
}

// Получить репорт
function getAsyncReportCauseCount(start, stop, callback) {
    $.ajax({
        type: 'GET',
            url: '../../api/dt/marriage_work/cause_count/start/' +toISOStringTZ(start).substring(0, 19) + '/stop/' +toISOStringTZ(stop).substring(0, 19),
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
        }
    });
}
// Получить репорт
function getAsyncReportDistrictCount(start, stop, callback) {
    $.ajax({
        type: 'GET',
        url: '../../api/dt/marriage_work/district_count/start/' + toISOStringTZ(start).substring(0, 19) + '/stop/' + toISOStringTZ(stop).substring(0, 19),
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
        }
    });
}