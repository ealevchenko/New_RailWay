//  
function getObjects(obj, key, val) {
    var objects = [];
    for (var i in obj) {
        if (!obj.hasOwnProperty(i)) continue;
        if (typeof obj[i] == 'object') {
            objects = objects.concat(getObjects(obj[i], key, val));
        } else if (i == key && obj[key] == val) {
            objects.push(obj);
        }
    }
    return objects;
}
// Получить значение атрибута text по атрибуту value
function getTextOption(obj, val) {
    if (obj != null) {
        for (var i = 0; i < obj.length; i++) {
            if (obj[i].value == val) return obj[i].text;
        }
    }
    return val;
}
