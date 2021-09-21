import { OrganizationModel } from "./OrganizationModel";
import Repository from "../Repository/Repository";
import "./organization.css"

interface OrganizationModelProps {
    org: OrganizationModel
}

function Organization(props: OrganizationModelProps) {
    return (
        <div>
            <div className="orgName">{props.org.name}</div>
            {
                props.org.repositories.map(r => (
                    <Repository key={r.id} orgName={props.org.name} repo={r} isProtected={r.isProtected}  />
                ))
            }
        </div>
    );
}
export default Organization;