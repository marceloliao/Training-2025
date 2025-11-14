import { useReducer } from "react";
import { createRoot } from "react-dom/client";

const initialScore = [
    {
        id: 1,
        score: 5,
        name: "John"
    },
    {
        id: 2,
        score: 0,
        name: "Sally"
    },
];

const reducer = (state, action) => {
    switch (action.type) {
        case "INCREASE":
            return state.map((player) => {
                if (player.id == action.id) {
                    return { ...player, score: player.score + 1 };
                }
                else {
                    return player;
                }
            });
        default:
            return state;
    }
};

function App() {
    const [score, dispatch] = useReducer(reducer, initialScore);

    const handleIncrease = (player) => {
        dispatch({ type: "INCREASE", id: player.id })
    };

    return (
        <>
            {score.map((player) => (
                <div key={player.id}>
                    <label>
                        <input type="button" onClick={() => handleIncrease(player)} value={player.name} />
                        &nbsp;{player.score}
                    </label>

                </div>
            ))}
        </>
    );
}

createRoot(document.getElementById('root')).render(
    <App />
) 