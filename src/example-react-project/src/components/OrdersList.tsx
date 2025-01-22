
export function OrdersList(){

    return <table>
        <thead>
            <tr>
                <th>ID</th>
                <th>Created On</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <th>1</th>
                <td>{new Date().toString()}</td>
            </tr>
        </tbody>
    </table>

}
