import React from "react";
import { Link } from "react-router-dom";

const ImportScanActions = ({
  isFormValid,
  onLinkClick,
  actionFormType,
  navigateURL,
}) => {
  return (
    <React.Fragment>
      <ul className="actions-list shipment-actions">
        <li>
          <Link
            to={navigateURL}
            /*   to={
              actionFormType === "importBox"
                ? `/received-box/import`
                : `/tag/scan`
            } */
          >
            {actionFormType === "importBox" ? (
              <div>
                <i className="ms-Icon ms-font-xl ms-Icon--Down i-size"></i>
                <p className="paragrapgh-shipment-action-alignment">Import</p>
              </div>
            ) : (
              <div>
                <i className="ms-Icon ms-font-xl ms-Icon--GenericScan i-size">
                  
                </i>
                <p className="paragrapgh-shipment-action-alignment">
                  Scan Tags
                </p>
              </div>
            )}
          </Link>
        </li>

        {/*  <li>
          <Link
            className={!isFormValid ? "disable-link" : null}
            onClick={e => onLinkClick({ targer: { action: "save" } })}
          >
            <div>
              <i
                className="ms-Icon ms-font-xl ms-Icon--Delete"
                aria-hidden="true"
              ></i>
              <p className="paragrapgh-shipment-action-alignment">Delete</p>
            </div>
          </Link>
        </li>

        <li>
          <Link
            className={!isFormValid ? "disable-link" : null}
            onClick={e => onLinkClick("back")}
          >
            <div>
              <i className="ms-Icon ms-font-xl ms-Icon--Refresh"></i>
              <p className="paragrapgh-shipment-action-alignment">Refresh</p>
            </div>
          </Link>
        </li>
        <li>
          <Link
            className={!isFormValid ? "disable-link" : null}
            onClick={e => onLinkClick("back")}
          >
            <div>
              <i className="ms-Icon ms-font-xl ms-Icon--ExcelDocument"></i>
              <p className="paragrapgh-shipment-action-alignment">
                Export To Excel
              </p>
            </div>
          </Link>
        </li> */}
      </ul>
    </React.Fragment>
  );
};

export default ImportScanActions;
