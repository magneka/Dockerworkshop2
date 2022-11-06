import { useEffect, useState } from "react";
import Table from 'react-bootstrap/Table';
import Card from 'react-bootstrap/Card';


const DatatableComponent = () => {

  const [data, setData] = useState([]);
  const URL = "http://localhost:5000/messages";

  useEffect(() => {
    fetchData();
  }, []);

  const fetchData = () => {
    fetch(URL)
      .then((res) => res.json())

      .then((response) => {
        console.log(response);
        setData(response);
      });
  };

  return (
    <div style={{ padding: '30px' }}>

      <Card border="secondary" style={{ width: '100%' }}>
        <Card.Header>
          <h1>Data fra ActiveMQ til MySQL til Python API til React</h1>
        </Card.Header>
        <Card.Body>
          <Card.Title>Secondary Card Title</Card.Title>
          <Card.Text>
            <div>

              <Table striped bordered hover size="sm">
                <thead>
                  <tr>
                    <th>Id</th>
                    <th>Fra navn</th>
                    <th>Epost</th>
                    <th>Melding</th>
                  </tr>
                </thead>
                <tbody>
                  {data.map((item, i) => (
                    <tr key={i}>
                      <td>{item.id}</td>
                      <td>{item.Username}</td>
                      <td>{item.UserEmail}</td>
                      <td>{item.MessageText}</td>
                    </tr>
                  ))}
                </tbody>
              </Table>
            </div>

          </Card.Text>
        </Card.Body>
      </Card>
    </div>
  );
}

export default DatatableComponent;
