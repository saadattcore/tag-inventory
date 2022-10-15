import React from "react";
import { Link } from "react-router-dom";
import config from "../../../config.json";

const IssuedBoxScanTagActions = ({
  isFormValid,
  onLinkClick,
  actionFormType,
  navigateURL,
  disableAssignTags,
  disableVerifyScanTag,
  tagsCount,
}) => {
  return (
    <React.Fragment>
      <ul className="actions-list shipment-actions">
        <li>
          <Link
            className={disableVerifyScanTag ? "disable-link" : null}
            onClick={(e) => onLinkClick({ target: { action: "scan" } })}
          >
            <div>
              <i
                className="ms-Icon ms-font-xl ms-Icon--GenericScan i-size"
                aria-hidden="true"
              ></i>
              <p className="paragrapgh-shipment-action-alignment">
                Scan Verify Tags
              </p>
            </div>
          </Link>
        </li>
        {/*  <li>
          <Link
            className={disableAssignTags ? "disable-link" : null}
            onClick={e => onLinkClick({ target: { action: "assign" } })}
            hidden={true}
          >
            <div>
              <i
                className="ms-Icon ms-font-xl ms-Icon--Add"
                aria-hidden="true"
              ></i>
              <p className="paragrapgh-shipment-action-alignment">
                Assign Tags
              </p>
            </div>
          </Link>
        </li> */}
        <li>
          <Link
            className={!isFormValid ? "disable-link" : null}
            onClick={(e) => onLinkClick({ targer: { action: "delete" } })}
          >
            <div>
              <i
                className="ms-Icon ms-font-xl ms-Icon--Delete i-size"
                aria-hidden="true"
              ></i>
              <p className="paragrapgh-shipment-action-alignment">Delete</p>
            </div>
          </Link>
        </li>
        <li className="tags-count-caption-li">
          <div className="tags-count-caption">{`${tagsCount} out of ${config.boxCount} Assigned`}</div>
        </li>
      </ul>
    </React.Fragment>
  );
};

export default IssuedBoxScanTagActions;
