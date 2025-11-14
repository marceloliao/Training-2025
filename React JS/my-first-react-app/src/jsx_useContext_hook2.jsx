// This time, we are using useContext hook
import { createRoot } from "react-dom/client";
import { useState, createContext, useContext } from "react";

const UserContext = createContext();

function Component1() {
    const [user, setUser] = useState("Linus");

    return (
        <UserContext.Provider value={user}>
            <h1>{`Hello ${user}`}</h1>
            {/* Call the second component  */}
            <Component2 />
        </UserContext.Provider>
    )
}

function Component2() {

    const message = `This is Component2, I don't need to receive a user parameter from Component1`;
    return (
        <>
            <h1>{message}</h1>
            {/* Call the third component  */}
            <Component3 />
        </>
    )
}

function Component3() {
    const user = useContext(UserContext);
    const message = `This is Component3, I don't need to receive a user parameter from Component2`;
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