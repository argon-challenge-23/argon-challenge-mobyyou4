import './App.css';
import useRepositories from './hooks/useRepositories';
import Organization from './Organization/Organization';

function App() {
  const org = useRepositories('argon-challenge-23');
  if (org)
    return (
      <div className="App">
        <Organization org={org} />
      </div>
    );

  return (<div>Org not found</div>)
}

export default App;
