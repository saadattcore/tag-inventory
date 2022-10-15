import React from "react";
import Dialog from "@material-ui/core/Dialog";
import DialogActions from "@material-ui/core/DialogActions";
import DialogContent from "@material-ui/core/DialogContent";
import DialogContentText from "@material-ui/core/DialogContentText";
import DialogTitle from "@material-ui/core/DialogTitle";
import Button from "@material-ui/core/Button";

const DialogComponent = ({ title, content, open, onDialogClose, actions }) => {
  return (
    <React.Fragment>
      <Dialog
        open={open}
        onClose={onDialogClose}
        aria-labelledby="alert-dialog-title"
        aria-describedby="alert-dialog-description"
        fullWidth={title ? true : null}
      >
        <DialogTitle id="alert-dialog-title">
          {title} <hr></hr>
        </DialogTitle>
        <DialogContent>
          <DialogContentText id="alert-dialog-description">
            {content}
          </DialogContentText>
        </DialogContent>
        <DialogActions>
          {actions.map((a) => (
            <Button
              onClick={(e) =>
                onDialogClose({
                  target: { action: a.action, elementType: a.elementType },
                })
              }
            >
              {a.text}
            </Button>
          ))}

          {/*    <Button
            onClick={(e) =>
              onDialogClose({
                target: { action: "continue", elementType: "dialog" },
              })
            }
            autoFocus
          >
            Save Tags
          </Button>
          <Button
            onClick={(e) =>
              onDialogClose({
                target: { action: "cancel", elementType: "dialog" },
              })
            }
          >
            Continue Tag Verification
          </Button> */}
        </DialogActions>
      </Dialog>
    </React.Fragment>
  );
};

export default DialogComponent;
