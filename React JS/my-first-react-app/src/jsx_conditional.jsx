import { createRoot } from 'react-dom/client'

function MissedGoal() {
    return <h1>MISSED!</h1>;
}

function MadeGoal() {
    return <h1>Goal!</h1>;
}

function Goal(props) {
    const isGoal = props.isGoal;
    if (isGoal) {
        return <MadeGoal />;
    }
    return <MissedGoal />;
}

createRoot(document.getElementById('root')).render(
    <Goal isGoal={false} />
);
