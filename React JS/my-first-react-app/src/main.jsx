import { createRoot } from 'react-dom/client';
import { useState } from 'react';

// const myElement = (
//   <table>
//     <tr>
//       <th>First Name</th>
//       <th>Last Name</th>
//     </tr>
//     <tr>
//       <td>John</td>
//       <td>Smith</td>
//     </tr>
//     <tr>
//       <td>Elsa</td>
//       <td>Snow</td>
//     </tr>
//   </table>
// )

// const myElement = (
//   <div>
//     <p>I am a paragraph.</p>
//     <p>I am a paragraph too.</p>
//   </div>
// );

const myElement = <h1>Hello {/* Wonderful */} World </h1>;

const fruitList = ["apple", "banana", "watermelon", "pinapple", "orange"];

function MyList() {
  return (
    <ul>
      {fruitList.map(fruit => <li key={fruit}>{fruit}</li>)}
    </ul>
  )
}

const users = [
  { id: 1, name: "John", age: 30 },
  { id: 2, name: "Mary", age: 45 },
  { id: 3, name: "Megan", age: 27 }
]

function UserList() {
  return (
    <ul>
      {users.map(user =>
        <li id={user.id}>{user.name} is {user.age} years old</li>
      )}
    </ul>
  )
}

// This function has 2 return, but since there is only one parameter, it can be simplified as above
// function UserList() {
//   return (
//     <ul>
//       {users.map(user => {
//         return (
//           <li id={user.id}>{user.name} is {user.age} years old</li>
//         );
//       })}
//     </ul>
//   )
// }

function App() {
  return (
    <ul>
      {fruitList.map((fruit, index, array) => {
        return (
          <li key={fruit}>
            Fruit: {fruit}, Index: {index}, Array: {array.map(x => x + "_")}
          </li>
        );
      })}
    </ul>
  );
}

// Using destructuring on Props, this is the preferred method
// function Greeting({ name, age }) {
//   return <h1>Hello, {name}! You are {age} years old.</h1>
// }

// Not using destructuring on Props
function Greeting(props) {
  return <h1>Hello, {props.name}! You are {props.age} years old.</h1>
}

function Counter() {

  // Destructuring the array returned by useState
  const [count, setCount] = useState(0);


  return (
    <button onClick={() => setCount(count + 1)}>Count: {count}</button>
  );
}

// function Greeting() {
//   return <h1>Hello, Marcelo! You are 55 years old.</h1>
// }

createRoot(document.getElementById('root')).render(
  myElement
)

// createRoot(document.getElementById('root')).render(
//   <MyList />
// )

// createRoot(document.getElementById('root')).render(
//   <UserList />
// )

// createRoot(document.getElementById('root')).render(
//   <App />
// )

// createRoot(document.getElementById('root')).render(
//   <Greeting name="Elizabeth" age={15} />
// )

// createRoot(document.getElementById('root')).render(
//   <Counter />
// )

// import { StrictMode } from 'react'
// import { createRoot } from 'react-dom/client'
// import './index.css'
// import App from './App.jsx'

// createRoot(document.getElementById('root')).render(
//   <StrictMode>
//     <App />
//   </StrictMode>
// )  