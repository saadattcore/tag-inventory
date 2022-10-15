import React from "react";
import { Link } from "react-router-dom";

const IssuedBoxDetailActions = ({
  isFormValid,
  onLinkClick,
  actionFormType,
  navigateURL,
  disableAssignTags,
  disableSerialList,
  nextStatus,
  disableCompleteBox
}) => {
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
            <div>
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
            <div className="back-action">
              <i className="ms-Icon ms-font-xl ms-Icon--SaveAndClose i-size"></i>
              <p className="paragrapgh-shipment-action-alignment">
                Save & Close
              </p>
            </div>
          </Link>
        </li>

        <li>
          <Link
            className={disableCompleteBox ? "disable-link image-disable" : null}
            onClick={e =>
              onLinkClick({ target: { action: "completebox", e: e } })
            }
          >
            <div>
              <i className="ms-Icon ms-font-xl ms-Icon--Giftbox i-size"></i>
              <p className="paragrapgh-shipment-action-alignment">
                {nextStatus}
              </p>
            </div>
          </Link>
        </li>
        <li>
          <Link
            className={disableSerialList ? "disable-link image-disable" : null}
            onClick={e => onLinkClick({ target: { action: "print", e: e } })}
          >
            <div>
              <i className="ms-Icon ms-font-xl ms-Icon--NumberedList i-size"></i>
              <p className="paragrapgh-shipment-action-alignment">
                {" "}
                Print Serial Number List
              </p>
            </div>
          </Link>
        </li>

        <li>
          <Link
            className={disableAssignTags ? "disable-link image-disable" : null}
            onClick={e =>
              onLinkClick({ target: { action: "assigntags", e: e } })
            }
          >
            <div>
              <i className="ms-Icon ms-font-xl ms-Icon--Tag i-size"></i>
              <p className="paragrapgh-shipment-action-alignment">
                {" "}
                Assign Tags
              </p>
            </div>
          </Link>
        </li>
        {/* <li>
          <Link>
            <span></span>
          </Link>
        </li>
        <li>
          <Link
            className={!isFormValid ? "disable-link" : null}
            onClick={e => onLinkClick({ target: { action: "refresh" } })}
          >
            <div>
              <i className="ms-Icon ms-font-xl ms-Icon--Refresh"></i>
              <p className="paragrapgh-shipment-action-alignment"> Referesh</p>
            </div>
          </Link>
        </li>
        <li>
          <Link
            className={!isFormValid ? "disable-link" : null}
            onClick={e => onLinkClick({ target: { action: "export" } })}
          >
            <div>
              <i className="ms-Icon ms-font-xl ms-Icon--ExcelDocument"></i>
              <p className="paragrapgh-shipment-action-alignment">
                {" "}
                Export to Excel
              </p>
            </div>
          </Link>
        </li> */}
      </ul>
    </React.Fragment>
  );
};

export default IssuedBoxDetailActions;
