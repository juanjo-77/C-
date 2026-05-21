import React, { useEffect, useState } from 'react'

const API_URL = import.meta.env.VITE_API_URL || 'http://localhost:5001'

function App() {
  const [productos, setProductos] = useState([])
  const [clientes, setClientes] = useState([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState(null)
  const [tab, setTab] = useState('productos')

  useEffect(() => {
    const fetchData = async () => {
      try {
        const [pRes, cRes] = await Promise.all([
          fetch(`${API_URL}/api/productos`),
          fetch(`${API_URL}/api/clientes`)
        ])
        if (!pRes.ok || !cRes.ok) throw new Error('Error al conectar con la API')
        setProductos(await pRes.json())
        setClientes(await cRes.json())
      } catch (e) {
        setError(e.message)
      } finally {
        setLoading(false)
      }
    }
    fetchData()
  }, [])

  return (
    <div style={{ fontFamily: 'Segoe UI, sans-serif', background: '#f1f5f9', minHeight: '100vh' }}>
      {/* Header */}
      <header style={{ background: '#0f172a', color: 'white', padding: '1rem 2rem', display: 'flex', alignItems: 'center', gap: '1rem' }}>
        <div style={{ background: '#2563eb', borderRadius: 10, width: 36, height: 36, display: 'flex', alignItems: 'center', justifyContent: 'center', fontWeight: 'bold' }}>F</div>
        <div>
          <div style={{ fontWeight: 'bold', fontSize: 18 }}>Firmeza</div>
          <div style={{ fontSize: 12, color: '#94a3b8' }}>Materiales de Construcción</div>
        </div>
      </header>

      <main style={{ maxWidth: 1000, margin: '2rem auto', padding: '0 1rem' }}>
        {/* Tabs */}
        <div style={{ display: 'flex', gap: 8, marginBottom: 24 }}>
          {['productos', 'clientes'].map(t => (
            <button key={t} onClick={() => setTab(t)}
              style={{
                padding: '8px 20px', borderRadius: 10, border: 'none', cursor: 'pointer',
                background: tab === t ? '#2563eb' : 'white',
                color: tab === t ? 'white' : '#64748b',
                fontWeight: tab === t ? 600 : 400,
                boxShadow: '0 1px 3px rgba(0,0,0,0.08)'
              }}>
              {t.charAt(0).toUpperCase() + t.slice(1)}
            </button>
          ))}
        </div>

        {loading && <div style={{ textAlign: 'center', color: '#64748b', padding: 40 }}>Cargando...</div>}
        {error && (
          <div style={{ background: '#fef2f2', border: '1px solid #fecaca', color: '#dc2626', borderRadius: 12, padding: 16 }}>
            ⚠️ {error} — Verifica que la API esté corriendo.
          </div>
        )}

        {!loading && !error && tab === 'productos' && (
          <div style={{ background: 'white', borderRadius: 16, boxShadow: '0 1px 3px rgba(0,0,0,0.08)', overflow: 'hidden' }}>
            <div style={{ padding: '16px 24px', borderBottom: '1px solid #f1f5f9', fontWeight: 600, color: '#1e293b' }}>
              Productos ({productos.length})
            </div>
            <table style={{ width: '100%', borderCollapse: 'collapse', fontSize: 14 }}>
              <thead>
                <tr style={{ background: '#f8fafc', color: '#64748b', fontSize: 12, textTransform: 'uppercase' }}>
                  <th style={{ padding: '10px 24px', textAlign: 'left' }}>#</th>
                  <th style={{ padding: '10px 24px', textAlign: 'left' }}>Nombre</th>
                  <th style={{ padding: '10px 24px', textAlign: 'left' }}>Categoría</th>
                  <th style={{ padding: '10px 24px', textAlign: 'right' }}>Precio</th>
                  <th style={{ padding: '10px 24px', textAlign: 'center' }}>Stock</th>
                </tr>
              </thead>
              <tbody>
                {productos.map(p => (
                  <tr key={p.id} style={{ borderTop: '1px solid #f8fafc' }}>
                    <td style={{ padding: '12px 24px', color: '#94a3b8' }}>{p.id}</td>
                    <td style={{ padding: '12px 24px', fontWeight: 600, color: '#1e293b' }}>{p.nombre}</td>
                    <td style={{ padding: '12px 24px' }}>
                      <span style={{ background: '#eff6ff', color: '#2563eb', borderRadius: 6, padding: '2px 10px', fontSize: 12 }}>{p.categoria}</span>
                    </td>
                    <td style={{ padding: '12px 24px', textAlign: 'right', fontWeight: 600 }}>${p.precio?.toLocaleString()}</td>
                    <td style={{ padding: '12px 24px', textAlign: 'center' }}>
                      <span style={{ background: p.stock < 10 ? '#fef2f2' : '#f0fdf4', color: p.stock < 10 ? '#dc2626' : '#16a34a', borderRadius: 20, padding: '2px 10px', fontSize: 12, fontWeight: 600 }}>{p.stock}</span>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        )}

        {!loading && !error && tab === 'clientes' && (
          <div style={{ background: 'white', borderRadius: 16, boxShadow: '0 1px 3px rgba(0,0,0,0.08)', overflow: 'hidden' }}>
            <div style={{ padding: '16px 24px', borderBottom: '1px solid #f1f5f9', fontWeight: 600, color: '#1e293b' }}>
              Clientes ({clientes.length})
            </div>
            <table style={{ width: '100%', borderCollapse: 'collapse', fontSize: 14 }}>
              <thead>
                <tr style={{ background: '#f8fafc', color: '#64748b', fontSize: 12, textTransform: 'uppercase' }}>
                  <th style={{ padding: '10px 24px', textAlign: 'left' }}>#</th>
                  <th style={{ padding: '10px 24px', textAlign: 'left' }}>Nombre</th>
                  <th style={{ padding: '10px 24px', textAlign: 'left' }}>Documento</th>
                  <th style={{ padding: '10px 24px', textAlign: 'left' }}>Correo</th>
                  <th style={{ padding: '10px 24px', textAlign: 'left' }}>Teléfono</th>
                </tr>
              </thead>
              <tbody>
                {clientes.map(c => (
                  <tr key={c.id} style={{ borderTop: '1px solid #f8fafc' }}>
                    <td style={{ padding: '12px 24px', color: '#94a3b8' }}>{c.id}</td>
                    <td style={{ padding: '12px 24px', fontWeight: 600, color: '#1e293b' }}>{c.nombre}</td>
                    <td style={{ padding: '12px 24px', color: '#64748b' }}>{c.documento}</td>
                    <td style={{ padding: '12px 24px', color: '#64748b' }}>{c.correo}</td>
                    <td style={{ padding: '12px 24px', color: '#64748b' }}>{c.telefono}</td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        )}
      </main>

      <footer style={{ textAlign: 'center', color: '#94a3b8', fontSize: 12, padding: '2rem' }}>
        © 2025 Firmeza — Materiales de Construcción
      </footer>
    </div>
  )
}

export default App