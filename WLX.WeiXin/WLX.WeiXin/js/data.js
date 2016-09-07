function getWebApiUrl() {
    return "http://114.55.66.247:7777";
}
function empty(v) {
    switch (typeof v) {
        case 'undefined':
            return true;
        case 'string':
            if (trim(v).length == 0)
                return true;
            break;
        case 'boolean':
            if (!v)
                return true;
            break;
        case 'number':
            if (0 === v)
                return true;
            break;
        case 'object':
            if (null === v)
                return true;
            if (undefined !== v.length && v.length == 0)
                return true;
            for (var k in v) {
                return false;
            }
            return true;
            break;
    }
    return false;
}

function trim(str) {
    for (var i = 0; i < str.length && str.charAt(i) == "  "; i++)
        ;
    for (var j = str.length; j > 0 && str.charAt(j - 1) == "  "; j--)
        ;
    if (i > j)
        return "";
    return str.substring(i, j);
}

function stringToJson(stringValue) {
    eval("var theJsonValue = " + stringValue);
    return theJsonValue;
}
//时间转换
function dataTostring(str) {
    if (str != null)
    {
        var d = str.replace("T", " ");
        return d1 = d.substring(0, d.length - 3)
    }
    else
        return " ";
}