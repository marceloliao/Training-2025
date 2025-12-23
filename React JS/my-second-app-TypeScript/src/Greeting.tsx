// Greeting.tsx
type GreetingProps = {
  name: string;
  age?: number;
};

function Greeting({ name, age }: GreetingProps) {
  return (
    <div>
      <h1>
        Hello {name}!{age !== undefined && <p>You are {age} years old</p>}
      </h1>
    </div>
  );
}

export default Greeting;
