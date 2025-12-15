"use strict";
// class Person {
//     private name: string;
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (Object.prototype.hasOwnProperty.call(b, p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        if (typeof b !== "function" && b !== null)
            throw new TypeError("Class extends value " + String(b) + " is not a constructor or null");
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
Object.defineProperty(exports, "__esModule", { value: true });
//     public constructor(name:string) {
//         this.name = name;
//     }
//     public getName():string {
//         return this.name;
//     }
// }
var Person = /** @class */ (function () {
    // private name: string;
    function Person(name) {
        this.name = name;
        this.name = name;
    }
    Person.prototype.getName = function () {
        return this.name;
    };
    return Person;
}());
var student = new Person("Samuel");
student.name = "Maria Pouzada";
console.log(student.getName());
;
var Rectangle = /** @class */ (function () {
    function Rectangle(width, height) {
        this.width = width;
        this.height = height;
    }
    Rectangle.prototype.getArea = function () {
        return this.width * this.height;
    };
    return Rectangle;
}());
var Square = /** @class */ (function (_super) {
    __extends(Square, _super);
    function Square(width) {
        return _super.call(this, width, width) || this;
    }
    return Square;
}(Rectangle));
var myRect = new Rectangle(20, 30);
console.log(myRect.getArea());
var mySquare = new Square(60);
console.log(mySquare.getArea());
var Polygon = /** @class */ (function () {
    function Polygon() {
    }
    Polygon.prototype.toString = function () {
        return "Polygon[area=".concat(this.getArea(), "]");
    };
    return Polygon;
}());
var Rectangle2 = /** @class */ (function (_super) {
    __extends(Rectangle2, _super);
    function Rectangle2(width, height) {
        var _this = _super.call(this) || this;
        _this.width = width;
        _this.height = height;
        return _this;
    }
    Rectangle2.prototype.getArea = function () {
        return this.width * this.height;
    };
    return Rectangle2;
}(Polygon));
var myRect2 = new Rectangle2(24, 16);
console.log("Rect2 area is ".concat(myRect2.getArea()));
console.log(myRect2.toString());
