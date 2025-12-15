"use strict";
function createPair(v1, v2) {
    return [v1, v2];
}
console.log(createPair("hello", 45));
var NamedValue = /** @class */ (function () {
    function NamedValue(name) {
        this.name = name;
    }
    NamedValue.prototype.setValue = function (value) {
        this._value = value;
    };
    NamedValue.prototype.getValue = function () {
        return this._value;
    };
    NamedValue.prototype.toString = function () {
        return "".concat(this.name, " is ").concat(this._value);
    };
    return NamedValue;
}());
var myValue = new NamedValue("MyValue");
myValue.setValue(4);
console.log(myValue.getValue());
console.log(myValue.toString());
