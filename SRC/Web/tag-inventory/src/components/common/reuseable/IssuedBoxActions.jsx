import React from "react";
import { Link } from "react-router-dom";

const IssuedBoxActions = ({
  isFormValid,
  onLinkClick,
  actionFormType,
  navigateURL,
  readOnly
}) => {
  return (
    <React.Fragment>
      <ul className="actions-list shipment-actions">
        <li>
          <Link to="/issued-box/create">
            <div>
              <i
                className="ms-Icon ms-font-xl ms-Icon--Add i-size"
                aria-hidden="true"
              ></i>
              <p className="paragrapgh-shipment-action-alignment">Create Box</p>
            </div>
          </Link>
        </li>
        <li>
          <Link
            className={readOnly ? "disable-link image-disable" : null}
            onClick={e => onLinkClick("completebox")}
          >
            <div>
              <i className="ms-Icon ms-font-xl ms-Icon--Giftbox i-size"></i>
              <p className="paragrapgh-shipment-action-alignment">
                Complete Box
              </p>
            </div>
          </Link>
        </li>
        <li>
          <Link onClick={e => onLinkClick("printlabel")}>
            <div>
              <i className="ms-Icon ms-font-xl ms-Icon--Print i-size"></i>
              <p className="paragrapgh-shipment-action-alignment">
                Print Label
              </p>
            </div>
          </Link>
        </li>

        <li>
          <Link onClick={e => onLinkClick("sendToPress")}>
            <div>
              <i
                className="ms-Icon ms-font-xl ms-Icon--PrintfaxPrinterFile i-size"
                aria-hidden="true"
              ></i>
              <p className="paragrapgh-shipment-action-alignment">
                Send To Press
              </p>
            </div>
          </Link>
        </li>
        <li>
          <Link onClick={e => onLinkClick("boxToDistributor")}>
            <div>
              <i className="ms-Icon ms-font-xl ms-Icon--GiftboxOpen i-size"></i>
              <p className="paragrapgh-shipment-action-alignment">
                Issue Boxes
              </p>
            </div>
          </Link>
        </li>

        <li>
          <Link to="/issued-box/verify-kitted-tags">
            <div>
              <i
                className="ms-Icon ms-font-xl ms-Icon--WorkItem i-size"
                aria-hidden="true"
              ></i>
              <p className="paragrapgh-shipment-action-alignment">
                Verify Kitted Tags
              </p>
            </div>
          </Link>
        </li>

        <li>
          <Link onClick={e => onLinkClick("serialList")}>
            <div>
              <i className="ms-Icon ms-font-xl ms-Icon--NumberedList i-size"></i>
              <p className="paragrapgh-shipment-action-alignment">
                Generate Serial Number List
              </p>
            </div>
          </Link>
        </li>
        {/*
        <li>
          <Link>
            <span></span>
          </Link>
        </li>
          <li>
          <Link onClick={e => onLinkClick("back")}>
            <div>
              <i className="ms-Icon ms-font-xl ms-Icon--Delete"></i>
              <p className="paragrapgh-shipment-action-alignment">Delete</p>
            </div>
          </Link>
        </li>
        <li>
          <Link onClick={e => onLinkClick("back")}>
            <div>
              <i className="ms-Icon ms-font-xl ms-Icon--Refresh"></i>
              <p className="paragrapgh-shipment-action-alignment">Refresh</p>
            </div>
          </Link>
        </li>
        <li>
          <Link onClick={e => onLinkClick("back")}>
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

export default IssuedBoxActions;
