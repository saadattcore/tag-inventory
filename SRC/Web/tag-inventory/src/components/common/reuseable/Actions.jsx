import React from "react";
import { Link } from "react-router-dom";

const Actions = ({ isFormValid, onLinkClick }) => {
  return (
    <React.Fragment>
      <ul className="actions-list shipment-actions">
        <li>
          <Link
            onClick={e => onLinkClick({ target: { action: "back", e: e } })}
          >
            <div title="Edit" className="back-action">
              <i
                className="ms-Icon ms-font-xl ms-Icon--ReplyAlt i-size"
                aria-hidden="true"
              ></i>
            </div>
          </Link>
        </li>

        <li>
          <Link
            className={!isFormValid ? "disable-link image-disable" : null}
            onClick={e => onLinkClick({ target: { action: "save", e: e } })}
          >
            <div title="Edit">
              <i className="ms-Icon ms-font-xl ms-Icon--Save i-size"></i>
              <p className="paragrapgh-shipment-action-alignment"> Save</p>
            </div>
          </Link>
        </li>
        <li>
          <Link
            className={!isFormValid ? "disable-link image-disable" : null}
            onClick={e =>
              onLinkClick({ target: { action: "saveclose", e: e } })
            }
          >
            <div>
              <i className="ms-Icon ms-font-xl ms-Icon--SaveAndClose i-size"></i>
              <p className="paragrapgh-shipment-action-alignment">
                Save & Close
              </p>
            </div>
          </Link>
        </li>
      </ul>
    </React.Fragment>
  );
};

export default Actions;
