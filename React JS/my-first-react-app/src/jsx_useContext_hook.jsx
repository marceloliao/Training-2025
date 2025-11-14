// To illustrate when not using useContext hook, using useState only
import { createRoot } from "react-dom/client";
import { useState } from "react";

function Component1() {
    const [user, setUser] = useState("Linus");
    return (
        <>
            <h1>{`Hello ${user}`}</h1>
            {/* Call the second component  */}
            <Component2 user={user} />
        </>
    )
}

function Component2({ user }) {

    const message = `This is Component2, I receive a {user} parameter from Component1`;
    return (
        <>
            <h1>{message}</h1>
            {/* Call the second component  */}
            <Component3 user={user} />
        </>
    )
}

function Component3({ user }) {

    const message = `This is Component3, I receive a {user} parameter from Component2`;
    return (
        <>
            <h1>{message}</h1>
            <h1>{`Hello ${user} again, we met in Component3 `}</h1>
        </>
    )
}

createRoot(document.getElementById('root')).render(
    <Component1 />
)