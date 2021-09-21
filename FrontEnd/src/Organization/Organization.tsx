import { OrganizationModel } from "./OrganizationModel";
import Repository from "../Repository/Repository";
import "./organization.css"

interface OrganizationModelProps {
    org: OrganizationModel
}

function Organization({org}: OrganizationModelProps) {
    return (
        <div className="orgWrapper">
            <div className="orgHeader">{org.name}</div>
            {
                org.repositories.map(r => (
                    <Repository key={r.id} orgName={org.name} repo={r} isProtected={r.isProtected}  />
                ))
            }
        </div>
    );
}
export default Organization;