"use strict";
function getDay() {
    return new Date().toTimeString();
}
console.log(getDay());
function divide(dividend, divisor) {
    return dividend / divisor;
}
console.log(divide(10, 2));
// Named parameters
function divide2(_a) {
    var dividend = _a.dividend, divisor = _a.divisor;
    return dividend / divisor;
}
console.log(divide2({ dividend: 15, divisor: 3 }));
// In this new function, the parameter `value` automatically gets assigned the type `number` from the type `Negate`
var negateFunction = function (value) { return value * -1; };
console.log(negateFunction(30));
