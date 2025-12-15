// class Person {
//     private name: string;

//     public constructor(name:string) {
//         this.name = name;
//     }

//     public getName():string {
//         return this.name;
//     }
// }
class Person {
    // private name: string;

    public constructor(public name:string) {
        this.name = name;
    }

    public getName():string {
        return this.name;
    }
}

const student = new Person("Samuel");
student.name = "Maria Pouzada";
console.log(student.getName()); 


interface Shape {
    getArea: () => number;
};

class Rectangle implements Shape {
    public constructor(protected readonly width:number, protected readonly height:number ) {}

    public getArea(): number {
        return this.width * this.height;
    }  
}

class Square extends Rectangle {
    public constructor (width:number){
        super (width, width);
    }
} 

const myRect = new Rectangle(20, 30);
console.log(myRect.getArea());

const mySquare = new Square(60);
console.log(mySquare.getArea());



abstract class Polygon {
    public abstract getArea(): number;
    
    public toString(): string {
        return `Polygon[area=${this.getArea()}]`;
    }
}

class Rectangle2 extends Polygon {
    public constructor(protected readonly width:number, protected readonly height:number) {
        super();
    }

    public getArea(): number {
        return this.width * this.height;
    }
}

const myRect2 = new Rectangle2(24, 16);
console.log(`Rect2 area is ${myRect2.getArea()}`);
console.log(myRect2.toString());

export {};