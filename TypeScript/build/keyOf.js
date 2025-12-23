"use strict";
function printPersonProperties(person, key) {
    console.log("Printing person with the key \"".concat(key, "\": ").concat(person[key]));
}
var person = { name: "Max", age: 45 };
printPersonProperties(person, "age");
function createStringPair(property, value) {
    var _a;
    return _a = {}, _a[property] = value, _a;
}
console.log(JSON.stringify(createStringPair("greeting", "hello")));
