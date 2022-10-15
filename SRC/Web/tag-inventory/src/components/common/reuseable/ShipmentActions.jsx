import React from "react";
import { Link } from "react-router-dom";

const ShipmentActions = (props) => {
  return (
    <React.Fragment>
      <ul className="actions-list shipment-actions">
        <li>
          <Link to="/shipment/create">
            <div>
              <i
                className="ms-Icon ms-font-xl ms-Icon--Add i-size"
                aria-hidden="true"
              ></i>
              <p className="paragrapgh-shipment-action-alignment">
                New Shipment
              </p>
            </div>
          </Link>
        </li>

        {/* <li>
          <Link>
            <div>
              <i
                className="ms-Icon ms-font-xl ms-Icon--Delete"
                aria-hidden="true"
              ></i>
              <p className="paragrapgh-shipment-action-alignment"> Delete</p>
            </div>
          </Link>
        </li>
        <li>
          <Link>
            <div>
              <i className="ms-Icon ms-font-xl ms-Icon--Print"></i>
              <p className="paragrapgh-shipment-action-alignment">
                Generate Serial Number List
              </p>
            </div>
          </Link>
        </li>

        <li>
          <Link>
            <div>
              <i className="ms-Icon ms-font-xl ms-Icon--Download"></i>
              <p className="paragrapgh-shipment-action-alignment">
                Generate Export Package Number List
              </p>
            </div>
          </Link>
        </li>

        <li>
          <Link>
            <div>
              <i className="ms-Icon ms-font-xl ms-Icon--Refresh"></i>
              <p className="paragrapgh-shipment-action-alignment">Refresh</p>
            </div>
          </Link>
        </li>
        <li>
          <Link>
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

export default ShipmentActions;
