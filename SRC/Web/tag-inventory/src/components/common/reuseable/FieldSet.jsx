import React from "react";

const FieldSet = ({
  inputValue,
  onTextChange,
  onButtonClick,
  form,
  buttonCaption,
  labelCaption,
  buttonCaptionSecond,
  showSecondBtn,
  disableSecondBtn,
  disableFirstBtn
}) => {
  return (
    <React.Fragment>
      <fieldset className="border p-2 area ">
        <legend className="w-auto spacing float-none">
          {form === "scanTag" ? "Scan Tag" : "New Box"}
        </legend>
        <div className="legend-content">
          <div className="row top-buffer">
            <div className="col-md-3 field-set-label">
              <label>{labelCaption}</label>
            </div>
            <div className="col-md-6 form-group">
              <input
                type="text"
                name="txtReceivedBox"
                className="form-control"
                autoFocus
                value={inputValue}
                onChange={onTextChange}
                number
              ></input>
            </div>
            <div className="col-md-2">
              {form === "scanTag" ? (
                <button
                  name="btnReceivedBox"
                  id="btnReceivedBox"
                  href="#"
                  className="btn ms-Icon ms-font-xl ms-Icon--GenericScan"
                  style={{ color: "#000000" }}
                  onClick={e => {
                    onButtonClick({
                      target: { name: "btnReceivedBox" }
                    });
                  }}
                ></button>
              ) : (
                <i className="ms-Icon ms-font-xl ms-Icon--GenericScan"></i>
              )}
            </div>
          </div>

          <div>
            <div className="row top-buffer">
              <div className="col-md-3">&nbsp;</div>
              <div className="col-md-6">
                {form === "issuedBox" && (
                  <button
                    className="btn btn-primary"
                    disabled={disableFirstBtn}
                    onClick={e => onButtonClick({ target: { name: "btnOne" } })}
                  >
                    {buttonCaption}
                  </button>
                )}
              </div>
            </div>
          </div>

          {showSecondBtn && (
            <div>
              <div className="row top-buffer">
                <div className="col-md-3">&nbsp;</div>
                <div className="col-md-6">
                  {form === "issuedBox" && (
                    <button
                      className="btn btn-primary"
                      onClick={e =>
                        onButtonClick({ target: { name: "btnTwo" } })
                      }
                      disabled={disableSecondBtn}
                    >
                      {buttonCaptionSecond}
                    </button>
                  )}
                </div>
              </div>
            </div>
          )}
        </div>
      </fieldset>
    </React.Fragment>
  );
};

export default FieldSet;
