import DatatableComponent from "./DatatableComponent";

function App() {

  return (
    <>
      <DatatableComponent />
      <div>
        <h1>Viktige Urler:</h1>
        <p>Contact form: <a href="http://localhost:3085/index.html" target="_blank">http://localhost:3085/index.html</a></p>        
        <p>SEQ Server: <a href="http://localhost:8095" target="_blank">http://localhost:8095</a></p>
        <p>ActiveMQ admin: <a href="http://localhost:8161/admin/browse.jsp?JMSDestination=Contact" target="_blank">http://localhost:8161/admin/browse.jsp?JMSDestination=Contact</a></p>
        <p>Python Rest api:  <a href="http://localhost:5000/messages" target="_blank">http://localhost:5000/messages</a></p>

      </div>
    </>
  )
}

export default App;
