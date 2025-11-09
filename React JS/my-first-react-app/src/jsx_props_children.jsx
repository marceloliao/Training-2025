import { createRoot } from 'react-dom/client'

function Son(props) {
    return (
        <div style={{ background: 'lightgreen' }}>
            <h2>Son</h2>
            <div>{props.children}</div>
        </div>
    );
}

function Daughter(props) {
    const { brand, model } = props;
    return (
        <div style={{ background: 'lightblue' }}>
            <h2>Daughter</h2>
            <div>{props.children}</div>
        </div>
    );
}

function Nephew(props) {
    const { brand, model } = props;
    return (
        <div style={{ background: 'lightpink' }}>
            <h2>Nephew</h2>
            <div>{props.children}</div>
        </div>
    );
}

function Parent() {
    return (
        <div>
            <h1>My two Children</h1>
            <Son>
                <p>
                    This was written in the Parent component,
                    but displayed as a part of the Son component
                </p>
            </Son>
            <Daughter>
                <p>
                    This was written in the Parent component,
                    but displayed as a part of the Daughter component
                </p>
            </Daughter>
            <Nephew>
                <p>
                    This was written in the Parent component,
                    but displayed as a part of the Nephew component
                </p>
            </Nephew>
        </div>
    );
}

createRoot(document.getElementById('root')).render(
    <Parent />
);
