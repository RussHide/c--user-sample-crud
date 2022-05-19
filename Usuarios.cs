using System;
using MySql.Data.MySqlClient;
using LibreriaAgenda;
using System.Windows.Forms;
using System.Collections.Generic;

namespace UserCRUD
{
    public partial class Usuarios : Form
    {


        public Usuarios()
        {
            InitializeComponent();
            dgvUsuarios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvUsuarios.MultiSelect = false;
            dgvUsuarios.RowPrePaint += new DataGridViewRowPrePaintEventHandler(dgvUsuarios_RowPrePaint);
            llenarTabla();
        }

        int selectedRow = -1;

        void llenarTabla()
        {
            dgvUsuarios.Rows.Clear();
            dgvUsuarios.Refresh();
            List<LibreriaAgenda.Usuario> lst = new LibreriaAgenda.ADOUser().datosTabla();
            foreach (var uss in lst)
            {
                DataGridViewRow newRow = new DataGridViewRow();
                newRow.CreateCells(dgvUsuarios);
                newRow.Cells[0].Value = uss.Id;
                newRow.Cells[1].Value = uss.Name;
                newRow.Cells[2].Value = uss.User;
                newRow.Cells[3].Value = "****"; 
                newRow.Cells[4].Value = uss.Add;
                newRow.Cells[5].Value = uss.Edit;
                newRow.Cells[6].Value = uss.Delete;
                dgvUsuarios.Rows.Add(newRow);
            }
        }

        void limpiarCampos()
        {
            txtNombre.Text = "";
            txtUsuario.Text = "";
            txtContraseña.Text = "";
            txtConfirmar.Text = "";
            chAgregar.Checked = false;
            chEditar.Checked = false;
            chEliminar.Checked = false;
            lblId.Text = "";
        }


        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (txtConfirmar.Text == "" && txtNombre.Text == "" && txtContraseña.Text == "" && txtUsuario.Text == "")
            {
                MessageBox.Show("todos los campos son obligatorios");
                return;
            }
            else
            {
                if (!chAgregar.Checked && !chEditar.Checked && !chEliminar.Checked)
                {
                    MessageBox.Show("seleccione almenos un permiso");
                    return;

                }
                else
                {
                    LibreriaAgenda.Usuario us = new LibreriaAgenda.Usuario();
                    us.Name = txtNombre.Text;
                    us.User = txtUsuario.Text;

                    try
                    {
                        if (txtContraseña.Text != txtConfirmar.Text)
                        {
                            MessageBox.Show("Las contraseñas no son iguales, vuelve a intentarlo");
                            return;
                        }
                        else
                        {
                            us.Password = txtContraseña.Text;
                        }
                        if (chAgregar.Checked)
                        {
                            us.Add = "Si";
                        }
                        else
                        {
                            us.Add = "No";

                        }
                        if (chEditar.Checked)
                        {
                            us.Edit = "Si";

                        }
                        else
                        {
                            us.Edit = "No";
                        }
                        if (chEliminar.Checked)
                        {
                            us.Delete = "Si";
                        }
                        else
                        {
                            us.Delete = "No";
                        }

                        //string query = "Insert into usuario (nombre,usuario,contrasena,agregar,editar,eliminar) values('" + txtNombre.Text + "','" + txtUsuario.Text + "','" + txtContraseña.Text + "','" + agregar + "','" + editar + "','" + eliminar + "')";
                        Console.WriteLine(us);
                        Console.WriteLine(us.Name);
                        Console.WriteLine(us.User);
                        Console.WriteLine(us.Password);
                        Console.WriteLine(us.Add);
                        bool proc = new LibreriaAgenda.ADOUser().addUser(us);
                        
                        if (proc)
                        {
                            llenarTabla();
                            MessageBox.Show("Se agrego correctamente");
                        }
                        else
                        {
                            MessageBox.Show("Hubo un error al agegar el usuario");
                        }
                        llenarTabla();
                    }
                    catch (MySqlException ex)
                    {
                        throw ex;
                    }
                }
            }
            limpiarCampos();
        }


        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (selectedRow != -1)
            {
                if (txtConfirmar.Text == "" && txtNombre.Text == "" && txtContraseña.Text == "" && txtUsuario.Text == "")
                {
                    MessageBox.Show("todos los campos son obligatorios para editar");
                    return;
                }
                else
                {
                    if (!chAgregar.Checked && !chEditar.Checked && !chEliminar.Checked)
                    {
                        MessageBox.Show("seleccione almenos un permiso");
                        return;

                    }
                    else
                    {
                        try
                        {
                            LibreriaAgenda.Usuario us = new LibreriaAgenda.Usuario();
                            us.Name = txtNombre.Text;
                            us.User = txtUsuario.Text;
                            if (txtContraseña.Text != txtConfirmar.Text)
                            {
                                MessageBox.Show("Las contraseñas no son iguales, vuelve a intentarlo");
                                return;
                            }
                            else
                            {
                                us.Password = txtContraseña.Text;
                            }
                            if (chAgregar.Checked)
                            {
                                us.Add = "Si";
                            }
                            else
                            {
                                us.Add = "No";

                            }
                            if (chEditar.Checked)
                            {
                                us.Edit = "Si";
                            }
                            else
                            {
                                us.Edit = "No";
                            }
                            if (chEliminar.Checked)
                            {
                                us.Delete = "Si";
                            }
                            else
                            {
                                us.Delete = "No";
                            }

                            bool proc = new LibreriaAgenda.ADOUser().editUser(us, Convert.ToInt32(lblId.Text));
                            MessageBox.Show("Se edito correctamente");
                            llenarTabla();
                        }
                        catch (MySqlException ex)
                        {
                            throw ex;
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Debes seleccionar un registro");
            }
            limpiarCampos();

        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (selectedRow != -1)
            {
                try
                {
                    bool proc = new LibreriaAgenda.ADOUser().deleteUser(Convert.ToInt32(lblId.Text));
                    MessageBox.Show("Se elimino correctamente");
                    llenarTabla();
                }
                catch (MySqlException ex)
                {
                    throw ex;
                }
            }
            else
            {
                MessageBox.Show("Debes seleccionar un registro");
            }
            limpiarCampos();
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
        }

        private void dgvUsuarios_Click(object sender, EventArgs e)
        {
            chAgregar.Checked = false;
            chEditar.Checked = false;
            chEliminar.Checked = false;
            selectedRow = dgvUsuarios.CurrentCell.RowIndex;
            lblId.Text = dgvUsuarios.CurrentRow.Cells[0].Value.ToString();
            txtNombre.Text = dgvUsuarios.CurrentRow.Cells[1].Value.ToString();
            txtUsuario.Text = dgvUsuarios.CurrentRow.Cells[2].Value.ToString();
            txtContraseña.Text = dgvUsuarios.CurrentRow.Cells[3].Value.ToString();
            if (dgvUsuarios.CurrentRow.Cells[4].Value.ToString() == "Si")
            {
                chAgregar.Checked = true;
            }
            if (dgvUsuarios.CurrentRow.Cells[5].Value.ToString() == "Si")
            {
                chEditar.Checked = true;
            }
            if (dgvUsuarios.CurrentRow.Cells[6].Value.ToString() == "Si")
            {
                chEliminar.Checked = true;
            }
        }

        private void dgvUsuarios_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            e.PaintParts &= ~DataGridViewPaintParts.Focus;
        }
    }
}
