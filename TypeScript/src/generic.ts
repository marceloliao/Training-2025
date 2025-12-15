function createPair<S, T>(v1: S, v2: T): [S, T] {
  return [v1, v2];
}

console.log(createPair<string, number>("hello", 45));

class NamedValue<T> {
  private _value: T | undefined;
  private name: string;

  constructor(name: string) {
    this.name = name;
  }

  public setValue(value: T) {
    this._value = value;
  }

  public getValue(): T | undefined {
    return this._value;
  }

  public toString(): string {
    return `${this.name} is ${this._value}`;
  }
}

const myValue = new NamedValue<number>("MyValue");
myValue.setValue(4);
console.log(myValue.getValue());
console.log(myValue.toString());
